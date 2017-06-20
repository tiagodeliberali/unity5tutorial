using UnityEngine;
using UnitySocketIO.Events;

public class PlayerMovement
{
    public string SessionId;
    public float X;
    public float Z;
    public float RY;
    public bool IsRunning;
    public float LV;
    public float LH;

    public PlayerMovement()
    {
        SessionId = string.Empty;
    }

    public PlayerMovement(string sessionId, float x, float z, float ry, bool isRunning, float lh, float lv)
    {
        SessionId = sessionId;
        X = x;
        Z = z;
        RY = ry;
        IsRunning = isRunning;
        LV = lv;
        LH = lh;
    }

    public PlayerMovement(SocketIOEvent obj)
    {
        var entity = JsonUtility.FromJson<PlayerMovement>(obj.data);

        SessionId = entity.SessionId;

        X = entity.X;
        Z = entity.Z;
        RY = entity.RY;

        IsRunning = entity.IsRunning;
        LH = entity.LH;
        LV = entity.LV;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }

    public bool MovementIsEqual(PlayerMovement mov)
    {
        return LH == mov.LH
            && LV == mov.LV;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != typeof(PlayerMovement))
            return false;

        var anotherMovement = obj as PlayerMovement;

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
            hash = (hash * 16777619) ^ LH.GetHashCode();
            hash = (hash * 16777619) ^ LV.GetHashCode();

            return hash;
        }
    }
}
