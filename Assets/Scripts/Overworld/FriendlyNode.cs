﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyNode : NodeStructure, IRadialArea
{
    private static string _type = "Friendly";
    private static Sprite _sprite = Resources.Load<Sprite>("Sprites/friendly-node-01");
    System.Random rand = new System.Random();

    private float _radius;
    private RadialArea _radialArea;
    private float _cooldown = 10;

    public override string Type 
    {
        get => _type;
    }
    public override Sprite Sprite
    {
        get => _sprite;
    }

    public float Radius {
        get => _radius;
        set => _radius = value;
    }
    public RadialArea RadialArea {
        get => _radialArea;
        set => _radialArea = value;
    }

    // Clientside cooldown for clicking on the node?
    public float CoolDown
    {
        get => _cooldown;
        set => _cooldown = value;
    }

    // Constructor (called BEFORE attached to a Node)
    public FriendlyNode()
    {

    }

    public override void AttachToNode(GameObject node)
    {
        try
        {
            GameObject radialArea = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/RadialArea"));
            radialArea.transform.SetParent(node.transform, true);
            radialArea.GetComponent<RadialArea>().DrawAreaOfEffect();
        }
        catch (Exception e) { Debug.Log(e); }
    }

    public override void nAttachToNode(GameObject node)
    {
        // Obtain reference to the Node's sprite child and set it to the proper sprite
        GameObject nodeSprite = node.transform.Find("NodeSprite").gameObject;
        SpriteRenderer nsRenderer = nodeSprite.GetComponent<SpriteRenderer>();
        nsRenderer.sprite = Sprite;
        nodeSprite.transform.localPosition = Vector3.zero;
        nodeSprite.transform.localScale = Vector3.one;

        if(node.transform.Find("RadialArea(Clone)") == null)
        {
            // Attach a RadialArea to the Node
            GameObject radialArea = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/RadialArea"));
            _radialArea = radialArea.GetComponent<RadialArea>();

            radialArea.transform.SetParent(node.transform, true);
            _radialArea.Radius = 100.0f * (float)GpsUtility.UnityUnitsPerMeter(GpsUtility.Map.gameObject);
            _radialArea.DrawAreaOfEffect();

            // Subscribe to RadialArea events
            SubscribeEnter();
            SubscribeExit();
        }        
    }

    public override void OnClicked(string nodeIdentifier)
    {
        Debug.Log("Friendly OnClicked!");

        /*
         * Two different ways we can handle this. 
         * 1) Spawn gold prefabs to be picked up/clicked on around the node by using RadialArea.SpawnObjectInRange.
         * 2) Just reward player for clicking on node. NOT IMPLEMENTED.
         */

        // TODO: Uncomment section below once we have a gold prefab.
        
        
        int goldSpawned = rand.Next(3, 10);
        for(int i = 0; i < goldSpawned; i++)
        {
            GameObject goldPrefab = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/VirusLight"));
            RadialArea.SpawnObjectInRange(goldPrefab);
        }


    }

    public void UpdateAction()
    {
        // TODO: Behavior that is called while the player is within the RadialArea
    }

    // Event handling for player entering the RadialArea.
    public void OnEnterAction(object sender, EventArgs e)
    {
        // TODO: Behavior when player enters this RadialArea
        Debug.Log("FriendlyNode.OnEnterAction!");
        _radialArea.GetComponent<LineRenderer>().material.SetColor("_Color", Color.red);
        _radialArea.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", Color.red);
    }
    public void SubscribeEnter()
    {
        RadialArea.EnteredArea += OnEnterAction;
    }
    public void UnsubscribeEnter()
    {
        RadialArea.EnteredArea -= OnEnterAction;
    }

    // Event handling for player exiting the RadialArea.
    public void OnExitAction(object sender, EventArgs e)
    {
        // TODO: Behavior when player exits this RadialArea
        Debug.Log("FriendlyNode.OnExitAction!");
        _radialArea.GetComponent<LineRenderer>().material.SetColor("_Color", Color.green);
        _radialArea.GetComponent<LineRenderer>().material.SetColor("_EmissionColor", Color.green);
    }
    public void SubscribeExit()
    {
        RadialArea.ExitedArea += OnExitAction;
    }
    public void UnsubscribeExit()
    {
        RadialArea.ExitedArea -= OnExitAction;
    }
}
