using Newtonsoft.Json;
using UnityEngine;
using UnityJSONExporter;

public class AtmosphereSkyboxController : TimelineBindingObjectBase
{
    // Sun parameters

    // render mesh enabled
    [JsonProperty("r_on")]
    public float RenderMeshEnabled = 1.0f;

    [JsonProperty("i_b")]
    public float BaseIntensity = 0.2f;

    [JsonProperty("i_s")]
    public float SpecularIntensity = 0.2f;

    [Space(13)]
    [JsonProperty("sp")]
    public Vector3 SunPosition = new Vector3(1, 1, 0);

    // 0 ~ 50
    [JsonProperty("si")]
    public float SunIntensity = 22.0f;

    // 1e6 ~ 10e6
    [JsonProperty("pr")]
    public float PlanetRadius = 6371e3f; // Earth radius + Geostationary orbit altitude

    // 1e6 ~ 10e6
    [JsonProperty("ar")]
    public float AtmosphereRadius = 6471e3f; // Earth radius +

    // 0 ~ 10000
    [JsonProperty("ca")]
    public float CameraAltitude = 100.0f;

    // 0 ~ 50e-6
    [JsonProperty("rc")]
    public Vector3 RayleighCoefficient = new Vector3(5.5e-6f, 13.0e-6f, 22.4e-6f);

    // 1000 ~ 20000
    [JsonProperty("rsh")]
    public float RayleighScaleHeight = 8000.0f;

    // 0 ~ 50e-6
    [JsonProperty("mc")]
    public float MieCoefficient = 21e-6f;

    // 100 ~ 5000
    [JsonProperty("msh")]
    public float MieScaleHeight = 1200.0f;

    // 0 ~ 1
    [JsonProperty("msd")]
    public float MieScatteringDirection = 0.758f;


    public override string ResolvePropertyName(string propertyName)
    {
        var res = JsonUtilities.ResolveJsonProperty(this, propertyName);
        LoggerProxy.Log($"[AtmosphereSkyboxController] type: {this.GetType()},ResolvePropertyName: {propertyName}, res: {res}");
        return res;
    }
}
