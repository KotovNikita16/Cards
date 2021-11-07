using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineExtension
{
    static private readonly Dictionary<string, int> runners = new Dictionary<string, int>();

	public static void parallelCoroutinesGroup(this IEnumerator coroutine, MonoBehaviour parent, string groupName)
	{
		if (!runners.ContainsKey(groupName))
			runners.Add(groupName, 0);

		runners[groupName]++;
		parent.StartCoroutine(doParallel(coroutine, parent, groupName));
	}

	static IEnumerator doParallel(IEnumerator coroutine, MonoBehaviour parent, string groupName)
	{
		yield return parent.StartCoroutine(coroutine);
		runners[groupName]--;
	}

	public static bool GroupProcessing(string groupName)
	{
		return (runners.ContainsKey(groupName) && runners[groupName] > 0);
	}

	public static void routinesStopped()
    {
		runners.Clear();
    }
}
