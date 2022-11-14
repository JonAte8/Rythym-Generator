using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OtherSettingsSO : ScriptableObject
{
	[SerializeField]
	private bool _swing;

	public bool Swing
	{
		get { return _swing; }
		set { _swing = value; }
	}

	[SerializeField]
	private bool _condenseMeasures;

	public bool CondenseMeasures
	{
		get { return _condenseMeasures; }
		set { _condenseMeasures = value; }
	}

	[SerializeField]
	private bool _playMetWithRythym;

	public bool PlayMetWithRythym
	{
		get { return _playMetWithRythym; }
		set { _playMetWithRythym = value; }
	}

	[SerializeField]
	private int _tempo;

	public int Tempo
	{
		get { return _tempo; }
		set { _tempo = value; }
	}

	[SerializeField]
	private int _totalmeasuresdropdown;

	public int TotalMeasuresDropDown
	{
		get { return _totalmeasuresdropdown; }
		set { _totalmeasuresdropdown = value; }
	}

	[SerializeField]
	private int _timesignaturedropdown;

	public int TimeSignatureDropDown
	{
		get { return _timesignaturedropdown; }
		set { _timesignaturedropdown = value; }
	}

}
