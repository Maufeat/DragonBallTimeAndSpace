// dnSpy decompiler from Assembly-CSharp.dll class: AkGameObjEnvironmentData
using System;
using System.Collections.Generic;
using UnityEngine;

public class AkGameObjEnvironmentData
{
	private void AddHighestPriorityEnvironmentsFromPortals(Vector3 position)
	{
		for (int i = 0; i < this.activePortals.Count; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				AkEnvironment akEnvironment = this.activePortals[i].environments[j];
				if (akEnvironment != null)
				{
					int num = this.activeEnvironmentsFromPortals.BinarySearch(akEnvironment, AkEnvironment.s_compareByPriority);
					if (num >= 0 && num < 4)
					{
						this.auxSendValues.Add(akEnvironment.data.Id, this.activePortals[i].GetAuxSendValueForPosition(position, j));
						if (this.auxSendValues.isFull)
						{
							return;
						}
					}
				}
			}
		}
	}

	private void AddHighestPriorityEnvironments(Vector3 position)
	{
		if (!this.auxSendValues.isFull && this.auxSendValues.Count() < this.activeEnvironments.Count)
		{
			for (int i = 0; i < this.activeEnvironments.Count; i++)
			{
				AkEnvironment akEnvironment = this.activeEnvironments[i];
				uint id = akEnvironment.data.Id;
				if ((!akEnvironment.isDefault || i == 0) && !this.auxSendValues.Contains(id))
				{
					this.auxSendValues.Add(id, akEnvironment.GetAuxSendValueForPosition(position));
					if (akEnvironment.excludeOthers || this.auxSendValues.isFull)
					{
						break;
					}
				}
			}
		}
	}

	public void UpdateAuxSend(GameObject gameObject, Vector3 position)
	{
		if (!this.hasEnvironmentListChanged && !this.hasActivePortalListChanged && this.lastPosition == position)
		{
			return;
		}
		this.auxSendValues.Reset();
		this.AddHighestPriorityEnvironmentsFromPortals(position);
		this.AddHighestPriorityEnvironments(position);
		bool flag = this.auxSendValues.Count() == 0;
		if (!this.hasSentZero || !flag)
		{
			AkSoundEngine.SetEmitterAuxSendValues(gameObject, this.auxSendValues, (uint)this.auxSendValues.Count());
		}
		this.hasSentZero = flag;
		this.lastPosition = position;
		this.hasActivePortalListChanged = false;
		this.hasEnvironmentListChanged = false;
	}

	private void TryAddEnvironment(AkEnvironment env)
	{
		if (env != null)
		{
			int num = this.activeEnvironmentsFromPortals.BinarySearch(env, AkEnvironment.s_compareByPriority);
			if (num < 0)
			{
				this.activeEnvironmentsFromPortals.Insert(~num, env);
				num = this.activeEnvironments.BinarySearch(env, AkEnvironment.s_compareBySelectionAlgorithm);
				if (num < 0)
				{
					this.activeEnvironments.Insert(~num, env);
				}
				this.hasEnvironmentListChanged = true;
			}
		}
	}

	private void RemoveEnvironment(AkEnvironment env)
	{
		this.activeEnvironmentsFromPortals.Remove(env);
		this.activeEnvironments.Remove(env);
		this.hasEnvironmentListChanged = true;
	}

	public void AddAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal component = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (component != null)
		{
			this.activePortals.Add(component);
			this.hasActivePortalListChanged = true;
			for (int i = 0; i < 2; i++)
			{
				this.TryAddEnvironment(component.environments[i]);
			}
		}
		else
		{
			AkEnvironment component2 = environmentCollider.GetComponent<AkEnvironment>();
			this.TryAddEnvironment(component2);
		}
	}

	private bool AkEnvironmentBelongsToActivePortals(AkEnvironment env)
	{
		for (int i = 0; i < this.activePortals.Count; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				if (env == this.activePortals[i].environments[j])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RemoveAkEnvironment(Collider environmentCollider, Collider gameObjectCollider)
	{
		AkEnvironmentPortal component = environmentCollider.GetComponent<AkEnvironmentPortal>();
		if (component != null)
		{
			for (int i = 0; i < 2; i++)
			{
				AkEnvironment akEnvironment = component.environments[i];
				if (akEnvironment != null && !gameObjectCollider.bounds.Intersects(akEnvironment.GetCollider().bounds))
				{
					this.RemoveEnvironment(akEnvironment);
				}
			}
			this.activePortals.Remove(component);
			this.hasActivePortalListChanged = true;
		}
		else
		{
			AkEnvironment component2 = environmentCollider.GetComponent<AkEnvironment>();
			if (component2 != null && !this.AkEnvironmentBelongsToActivePortals(component2))
			{
				this.RemoveEnvironment(component2);
			}
		}
	}

	private readonly List<AkEnvironment> activeEnvironments = new List<AkEnvironment>();

	private readonly List<AkEnvironment> activeEnvironmentsFromPortals = new List<AkEnvironment>();

	private readonly List<AkEnvironmentPortal> activePortals = new List<AkEnvironmentPortal>();

	private readonly AkAuxSendArray auxSendValues = new AkAuxSendArray();

	private Vector3 lastPosition = Vector3.zero;

	private bool hasEnvironmentListChanged = true;

	private bool hasActivePortalListChanged = true;

	private bool hasSentZero;
}
