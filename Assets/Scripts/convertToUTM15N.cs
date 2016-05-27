/*
convertToBng.cs
This script is used to convert GPS coordinates
from the WGS84 geographic projection (LatLongs) into
the OSGB36 British National Grid map projection.

It can also be used to convert from BNG into Unity
gamespace coordinates by using a false Easting and Northing

This script is used to automatically update the virtual
position of the AR device from the real reality (GPS) position

It also builds the Graphical User Interface for the Location-Based AR application

EDITED BY AUSTIN MASON; changed from BNG to UTM zone 15N for Northfield, MN

*/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapCoordinate {
	public float latitude; 
	public float longitude; 
	public float altitude; 
	public float heading;

	public float x { 
		get {
			return longitude; 
		}

		set {
			longitude = value;
		}
	}

	public float y { 
		get {
			return latitude; 
		}

		set {
			latitude = value;
		}
	}

	public float z { 
		get {
			return altitude; 
		}

		set {
			altitude = value;
		}
	}

	public float direction { 
		get {
			return heading; 
		}

		set {
			heading = value;
		}
	}

	public MapCoordinate(float lon, float lat, float alt, float dir) {
		longitude = lon; 
		latitude = lat;
		altitude = alt;
		direction = dir;
	}
}

public class convertToUTM15N : MonoBehaviour {

	public float gpsAccuracy = 5;
	public float gpsUpdateDistance = 1;
	public float falseEasting = 487340;
	public float falseNorthing = 492247;
	private GameObject GyroCam;
	bool gpsRunning = false;
	private bool ready = false;
	public MapCoordinate globalPos;
	public MapCoordinate prevGlobalPos;
	public MapCoordinate BNGPos;

	private float BNG_E;
	private float BNG_N;
	private float BNG_alt;
	private float refresh_time = 2.0f;

	//GUI Variables
	private string BNG_E_input = "487757";
	private string BNG_N_input = "4923040";
	private string BNG_alt_input = "28730";
	private string BNG_heading_input = "180";

	public Shader diffuse; // = Shader.Find("Nature/Terrain/Standard");
	public Shader transwalls;// = Shader.Find("TransWalls");
	public Terrain landscape;
//	landscape = GameObject.Find("Terrain");
	public GameObject buildings;
//	buildings = GameObject.Find("Buildings");
	public GameObject spheres;
//	spheres = GameObject.Find("spheres");


	void Awake() {
		prevGlobalPos = new MapCoordinate(0,0,0,0);
		globalPos = new MapCoordinate(0,0,0,0);
		BNGPos = new MapCoordinate(0,0,0,0);
	}

	// Use this for initialization
	void Start () {

		StartGPS();
		LatLongToEastNorth(globalPos.latitude, globalPos.longitude, globalPos.altitude,	true);

//		landscape = GameObject.Find("Terrain");
//		buildings = GameObject.Find("Buildings");
//		spheres = GameObject.Find("spheres");
//		diffuse = Shader.Find("Nature/Terrain/Standard");
//		transwalls = Shader.Find("TransWalls");



	}

	// Update is called once per frame
	void Update () {

	}
	private double toRad(double val)
	{
		return val * (System.Math.PI / 180);
	}

	public void LatLongToEastNorth(double latitude, double longitude, double altitude, bool move_camera = false) 
	{
		//This will not work unless you have your lats and longs in decimal degrees.
		latitude = toRad(latitude);
		longitude = toRad(longitude);

//		double a = 6377563.396, b = 6356256.910; // Airy 1830 major &amp; minor semi-axes
		//double a = 6378137.0, b = 6356752.314245; WGS84 major &amp; minor semi-axes
		double a = 6378137.0, b = 6356752.314; // NAD83 major &amp; minor semi-axes

//		double F0 = 0.9996012717; // NatGrid scale factor on central meridian
		double F0 = 0.9996; // UTM scale factor on central meridian
//		double lat0 = toRad(49);
//		double lon0 = toRad(-2); // NatGrid true origin
		double lat0 = toRad(14.25);
		double lon0 = toRad(-96); // origin of bounds for NAD83/UTM Zone 15N: http://spatialreference.org/ref/epsg/nad83-utm-zone-15n/
		double N0 = -100000, E0 = 400000; // northing &amp; easting of true origin, metres
//		double N0 = 0, E0 = 0; // northing &amp; easting of true origin, metres
		double e2 = 1 - (b * b) / (a * a); // eccentricity squared
		double n = (a - b) / (a + b), n2 = n * n, n3 = n * n * n;

		double cosLat = System.Math.Cos(latitude), sinLat = System.Math.Sin(latitude);
		double nu = a * F0 / System.Math.Sqrt(1 - e2 * sinLat * sinLat) ; // transverse radius of curvature
		double rho = a * F0 * (1 - e2) / System.Math.Pow(1 - e2 * sinLat * sinLat, 1.5); // meridional radius of curvature

		double eta2 = nu / rho - 1;
		
		double Ma = (1 + n + (5 / 4) * n2 + (5 / 4) * n3) * (latitude - lat0);
		double Mb = (3 * n + 3 * n * n + (21/8)*n3) * System.Math.Sin(latitude - lat0) * System.Math.Cos(latitude + lat0);
		double Mc = ((15/8)*n2 + (15/8)*n3) * System.Math.Sin(2 * (latitude - lat0)) * System.Math.Cos(2 * (latitude + lat0));
		double Md = (35 / 24) * n3 * System.Math.Sin(3 * (latitude - lat0)) * System.Math.Cos(3 * (latitude + lat0));
		double M = b * F0 * (Ma - Mb + Mc - Md); // meridional arc 156
		
		double cos3lat = cosLat * cosLat * cosLat;
		double cos5lat = cos3lat * cosLat * cosLat;
		double tan2lat = System.Math.Tan(latitude) * System.Math.Tan(latitude);
		double tan4lat = tan2lat * tan2lat;

		double I = M + N0;
		double II = (nu / 2) * sinLat * cosLat;
		double III = (nu / 24) * sinLat * cos3lat * (5 - tan2lat + 9 * eta2);
		double IIIA = (nu / 720) * sinLat * cos5lat * (61 - 58 * tan2lat + tan4lat);
		double IV = nu * cosLat;
		double V = (nu / 6) * cos3lat * (nu / rho - tan2lat);
		double VI = (nu / 120) * cos5lat * (5 - 18 * tan2lat + tan4lat + 14 * eta2 - 58	* tan2lat * eta2);

		double dLon = longitude - lon0;
		double dLon2 = dLon * dLon, dLon3 = dLon2 * dLon, dLon4 = dLon3 * dLon, dLon5 = dLon4 * dLon, dLon6 = dLon5 * dLon;

		double N = I + II * dLon2 + III * dLon4 + IIIA * dLon6; //This is the northing
		double E = E0 + IV * dLon + V * dLon3 + VI * dLon5; //This is the easting
		BNGPos.x = (float)E;
		BNGPos.z = (float)N;
		BNGPos.y = (float)altitude;
		Debug.Log("BNG E: " + E + " BNG N: " + N + " Alt: " + altitude);
		if (move_camera) {
			MoveCameraToGameSpace(E, N, altitude);
		}

	}

	public void MoveCameraToGameSpace(double raw_BNG_E, double raw_BNG_N, double raw_BNG_alt, double heading = 9999.99){
		GyroCam = GameObject.Find("camGrandParent");
		Debug.Log("I should be moving");
		//now put the GyroCamera into the right place
		BNG_E = (float)raw_BNG_E - falseEasting;
		BNG_N = (float)raw_BNG_N - falseNorthing;
		BNG_alt = (float)raw_BNG_alt;
		//Vector3 pos = new Vector3(BNG_E, BNG_alt, BNG_N); 
		GyroCam.transform.position = new Vector3(BNG_E, BNG_alt, BNG_N); 
		if (heading != 9999.9) {
			GyroCam.transform.localEulerAngles = new Vector3 (0.0f, (float)heading, 0.0f);
			
			} else {
				GyroCam.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f); 
			}
		}

		IEnumerator ActivateGPS() {
			gpsRunning = true;
			Input.location.Start(gpsAccuracy, gpsUpdateDistance);

			float duration = 0;
			while (duration < 20.0f) {
				if (Input.location.status == LocationServiceStatus.Running
					|| Input.location.status == LocationServiceStatus.Failed) break;
				yield return new WaitForSeconds(0.1f); 

				duration += 0.1f;
			}

			if (duration >= 20.0f) {
				Debug.Log("**** LocationService Timed out"); 
			}
			
			if (Input.location.status == LocationServiceStatus.Failed) { 
				Debug.Log("**** User declined LocationService?");
				gpsRunning = false; 
			}
			
			ready = true; 
			Input.compass.enabled = true;
			
			while (Input.location.status == LocationServiceStatus.Running) { 
				globalPos.longitude = Input.location.lastData.longitude;
				globalPos.latitude = Input.location.lastData.latitude;
				globalPos.altitude = Input.location.lastData.altitude;
				//Debug.Log("Lat:" + globalPos.latitude + " Lon: " + globalPos.longitude + "Alt: " + globalPos.altitude);
				LatLongToEastNorth(globalPos.latitude, globalPos.longitude, globalPos.altitude);
				/*
				if (globalPos.x != prevGlobalPos.x
                    || globalPos.y != prevGlobalPos.y) Debug.Log("iphone gps: (" + globalPos.x + "," + globalPos.y + ")");
				*/
				
				yield return new WaitForSeconds(refresh_time); 
			}

			gpsRunning = false; 
		}
		
		public void StartGPS() {
			if (Application.isEditor) {
				ready = true;
				return; 
			}

			if (!gpsRunning) StartCoroutine(ActivateGPS()); 
		}

		public void OnGUI() {
			BNG_E_input = GUI.TextField(new Rect(10, 10, 200, 20),BNG_E_input, 50); 
			BNG_N_input = GUI.TextField(new Rect(10, 40, 200, 20),BNG_N_input, 50); 
			BNG_alt_input = GUI.TextField(new Rect(10, 70, 200, 20),BNG_alt_input, 50); 
			BNG_heading_input = GUI.TextField (new Rect (10, 110, 200, 20), BNG_heading_input, 50);

			if (GUI.Button (new Rect (10,140,200,100), "Reset Position and Heading")) { 
				MoveCameraToGameSpace(double.Parse(BNG_E_input), double.Parse(BNG_N_input),	double.Parse(BNG_alt_input), double.Parse(BNG_heading_input));
			}

		if (GUI.Button (new Rect (10, 240, 200, 100), "Show/Hide Landscape")) {
			if (landscape.GetComponent<Terrain>().materialTemplate.shader == diffuse)
				landscape.GetComponent<Terrain> ().materialTemplate.shader = transwalls;
			else
				landscape.GetComponent<Terrain> ().materialTemplate.shader = diffuse;
		}

		if (GUI.Button (new Rect (10,450,200,100), "Set position via GPS")) {
			LatLongToEastNorth(globalPos.latitude, globalPos.longitude, globalPos.altitude, false);
			BNG_E_input = GUI.TextField(new Rect(10, 10, 200, 20),"" + BNGPos.x, 50); 
			BNG_N_input = GUI.TextField(new Rect(10, 40, 200, 20),"" + BNGPos.z, 50); 
			BNG_alt_input = GUI.TextField(new Rect(10, 70, 200, 20),"" + BNGPos.y, 50);
		}

		//now put in the buttons for the test scenarios
		if (GUI.Button (new Rect (1350,140,100,50), "Off")) { 
			buildings.SetActive (false);
			spheres.SetActive (false); 
		}
		if (GUI.Button (new Rect (1350,210,100,50), "Spheres")) { 
			buildings.SetActive(false);
			spheres.SetActive(true); 
		}
		if (GUI.Button (new Rect (1350,280,100,50), "Buildings")) { 
			buildings.SetActive(true);
			spheres.SetActive(false); 
		}
	} 

}