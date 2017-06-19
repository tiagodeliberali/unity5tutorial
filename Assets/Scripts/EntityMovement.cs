public class EntityMovement
{
    public string SessionId { get; set; }
    public float X { get; set; }
    public float Z { get; set; }
    public float RY { get; set; }
    public bool IsRunning { get; set; }

    public EntityMovement()
    {
        SessionId = string.Empty;
    }

    public EntityMovement(string sessionId, float x, float z, float ry, bool isRunning)
    {
        SessionId = sessionId;
        X = x;
        Z = z;
        RY = ry;
        IsRunning = isRunning;
    }

    public JSONObject ToJSONObject()
    {
        return new JSONObject(ToString());
    }

    public override string ToString()
    {
        return string.Format(
            @"{{""s"":""{0}"",""x"":{1},""z"":{2},""a"":{3},""ry"":{4}}}",
            SessionId,
            X,
            Z,
            IsRunning,
            RY);
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
            hash = (hash * 16777619) ^ IsRunning.GetHashCode();

            return hash;
        }
    }
}
