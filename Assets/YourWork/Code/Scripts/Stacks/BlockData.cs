using System;
using Newtonsoft.Json;

[Serializable, JsonObject]
public class BlockData 
{
    [JsonProperty("id")] public int Id;
    [JsonProperty("subject")] public string Subject;
    [JsonProperty("grade")] public string Grade;
    [JsonProperty("mastery")] public int Mastery;
    [JsonProperty("domainid")] public string DomainId;
    [JsonProperty("domain")] public string Domain;
    [JsonProperty("cluster")] public string Cluster;
    [JsonProperty("standardid")] public string StandardId;
    [JsonProperty("standarddescription")] public string StandardDescription;
}
