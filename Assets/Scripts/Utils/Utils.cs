using System;
using System.Collections.Generic;
using UnityEngine;
public class Utils
{
    public static int ENEMIES = LayerMask.NameToLayer("Enemies");
    public static int MAP = LayerMask.NameToLayer("Map");

    public const float LOOK_AT_IMMEDIATLY = 1000f;

    public static bool IsMobile { get => SystemInfo.deviceType == DeviceType.Handheld; }

    public static bool IsPlayer(Collider other) => other.gameObject.name == "Player"; // other.gameObject == GameManager.Instance.Player.gameObject;

    public static Quaternion LookAt(Transform self, Transform target, float turnSpeed = LOOK_AT_IMMEDIATLY)
    {
        Vector3 targetDir = target.position - self.position;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(self.forward, targetDir, turnSpeed * Time.deltaTime, .0f);

        return Quaternion.LookRotation(newDir);
    }

    public static Quaternion LookAtAll(Transform self, Transform target, float turnSpeed = LOOK_AT_IMMEDIATLY)
    {
        Vector3 targetDir = target.position - self.position;
        Vector3 newDir = Vector3.RotateTowards(self.forward, targetDir, turnSpeed * Time.deltaTime, .0f);

        return Quaternion.LookRotation(newDir);
    }
    public static Quaternion LookAt(Transform self, Vector3 target, float turnSpeed)
    {
        Vector3 targetDir = target - self.position;
        targetDir.y = 0;
        Vector3 newDir = Vector3.RotateTowards(self.forward, targetDir, turnSpeed * Time.deltaTime, .0f);

        return Quaternion.LookRotation(newDir);
    }

    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    public static bool HasInternet() => !(Application.internetReachability == NetworkReachability.NotReachable);


    public static Vector3 GetRelativePosition(RectTransform p, Camera c, float DesireDistanceFromCamera = 5f)
    {
        return c.ScreenToWorldPoint(new Vector3(p.position.x, p.position.y, DesireDistanceFromCamera));
    }

    public static (string, string) NumberToOrdinal(int number)
    {
        if (number < 0) return (number.ToString(), "");

        long rem = number % 100;

        if (rem >= 11 && rem <= 13) return (number.ToString(), "th");

        return (number % 10) switch
        {
            1 => (number.ToString(), "st"),
            2 => (number.ToString(), "nd"),
            3 => (number.ToString(), "rd"),
            _ => (number.ToString(), "th"),
        };
    }

    public static string NowToDate() => DateTime.Now.ToString("yyyy-MM-dd");

    public static Transform GetClosestTransform(Transform self, Transform[] transforms)
    {
        if (transforms.Length == 0) return null;

        Vector3 currentPos = self.position;
        Transform enemy = transforms[0];
        var minDist = float.MaxValue;
        foreach (Transform other in transforms)
        {
            if (other.gameObject == self.gameObject)
                continue;

            var t = other.transform.position - currentPos;
            float dist = t.x * t.x + t.y * t.y + t.z * t.z;
            if (dist < minDist)
            {
                enemy = other;
                minDist = dist;
            }
        }

        return enemy;
    }

    public static T[] FindAllOfType<T>() where T : class
    {
        List<T> result = new List<T>();
        foreach (var monoBehaviour in GameObject.FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is T)
            {
                result.Add(monoBehaviour as T);
            }
        }
        return result.ToArray();
    }
}

public delegate void Notify();
