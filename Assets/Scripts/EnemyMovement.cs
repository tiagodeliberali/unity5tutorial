using UnityEngine;
using UnitySocketIO.Events;

public class EnemyMovement
{
    public string SessionId;
    public float X;
    public float Z;
    public float RY;

    public EnemyMovement()
    {
        SessionId = string.Empty;
    }

    public EnemyMovement(string sessionId, float x, float z, float ry)
    {
        SessionId = sessionId;
        X = x;
        Z = z;
        RY = ry;
    }

    public EnemyMovement(SocketIOEvent obj)
    {
        var entity = JsonUtility.FromJson<PlayerMovement>(obj.data);

        SessionId = entity.SessionId;

        X = entity.X;
        Z = entity.Z;
        RY = entity.RY;
    }

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
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

            return hash;
        }
    }
}
