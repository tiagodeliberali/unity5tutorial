using SocketIO;
using UnityEngine;

public class EntityMovement
{
    public string SessionId { get; set; }
    public float X { get; set; }
    public float Z { get; set; }
    public float RY { get; set; }
    public bool? IsRunning { get; set; }
    public float? LV { get; private set; }
    public float? LH { get; private set; }

    public EntityMovement()
    {
        SessionId = string.Empty;
    }

    public EntityMovement(string sessionId, float x, float z, float ry)
    {
        SessionId = sessionId;
        X = x;
        Z = z;
        RY = ry;
    }

    public EntityMovement(string sessionId, float x, float z, float ry, bool isRunning, float lh, float lv)
    {
        SessionId = sessionId;
        X = x;
        Z = z;
        RY = ry;
        IsRunning = isRunning;
        LV = lv;
        LH = lh;
    }

    public EntityMovement(SocketIOEvent obj)
    {
        SessionId = obj.data["s"].str;

        X = float.Parse(obj.data["x"].ToString());
        Z = float.Parse(obj.data["z"].ToString());
        RY = float.Parse(obj.data["ry"].ToString());

        if (obj.data.HasField("a"))
            IsRunning = bool.Parse(obj.data["a"].ToString());

        if (obj.data.HasField("lh"))
            LH = float.Parse(obj.data["lh"].ToString());

        if (obj.data.HasField("lv"))
            LV = float.Parse(obj.data["lv"].ToString());
    }

    public JSONObject ToJSONObject()
    {
        return new JSONObject(ToString());
    }

    public override string ToString()
    {
        return string.Format(
            @"{{""s"":""{0}"",""x"":{1},""z"":{2},""ry"":{3}{4}{5}{6}}}",
            SessionId,
            X,
            Z,
            RY,
            IsRunning.HasValue ? ",\"a\":" + IsRunning : string.Empty,
            LH.HasValue ? ",\"lh\":" + LH : string.Empty,
            LV.HasValue ? ",\"lv\":" + LV : string.Empty);
    }

    public bool MovementIsEqual(EntityMovement mov)
    {
        return LH == mov.LH
            && LV == mov.LV;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != typeof(EntityMovement))
            return false;

        var anotherMovement = obj as EntityMovement;

        return GetHashCode() == anotherMovement.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = (int)2166136261;

            // Suitable nullity checks etc, of course :)
            hash = (hash * 16777619) ^ (string.IsNullOrEmpty(SessionId) ? "SessionId".GetHashCode() : SessionId.GetHashCode());
            hash = (hash * 16777619) ^ X.GetHashCode();
            hash = (hash * 16777619) ^ Z.GetHashCode();
            hash = (hash * 16777619) ^ RY.GetHashCode();
            hash = (hash * 16777619) ^ (IsRunning.HasValue ? IsRunning.GetHashCode() : false.GetHashCode());
            hash = (hash * 16777619) ^ (LH.HasValue ? LH.GetHashCode() : false.GetHashCode());
            hash = (hash * 16777619) ^ (LV.HasValue ? LV.GetHashCode() : false.GetHashCode());
            return hash;
        }
    }
}
