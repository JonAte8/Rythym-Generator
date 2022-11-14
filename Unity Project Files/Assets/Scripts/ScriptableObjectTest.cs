using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableObjectTest : ScriptableObject
{
	[SerializeField]
	private bool[] _values;

	public bool[] Values
	{
		get { return _values; }
		set { _values = value; }
	}

}
