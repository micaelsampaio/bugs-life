using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Info")]
    public float Speed;
    public string FirstName;
    public string SurName;
    public int Population;
    public string Family;
    public int Age;
    public Gender Gender = Gender.MALE;

    [Header("Game")]
    public Animator Animator;
    public AI_Actions Action = AI_Actions.IDLE;
    public Renderer Renderer;
    private Vector3 TargetPosition = Vector3.zero;
    private float CurrentTime = 0f;
    private float Delay = 0f;
    public bool Ready = false;
    public Collectable Collectable;
    public Texture CurrentTexture;
    private Action<AI> ActionCb;

    [Header("Customize")]
    public GameObject AntennaeSocket;
    public List<CustomizeItemClone> CustomObjects = new List<CustomizeItemClone>();

    public bool Hat;
    public bool Hair;
    public bool Beard;
    public bool Antennea;

    private void OnEnable()
    {
        Ready = false;
        Animator.SetTrigger("Idle");
        Animator.SetBool("Walk", false);
        Action = AI_Actions.IDLE;
        CurrentTime = 0f;
        transform.localScale = Vector3.one;
    }

    public string FullName { get => $"{FirstName} {SurName}"; }

    public string AllInfo { get => $" {FullName} {Gender} {Age}, {Family}, {Population}"; }

    public void GoToNextPosition(Vector3 position, float delay = 1)
    {
        TargetPosition = position;
        CurrentTime = 0;
        Delay = delay;
        Action = AI_Actions.GO_TO_POS;
    }

    public void OfferAccepted(Vector3 position, Action<AI> cb)
    {
        CurrentTime = 0;
        Delay = 1;
        TargetPosition = new Vector3(position.x, transform.position.y, position.z);
        Action = AI_Actions.GO_TO_POS_CB;
        ActionCb = cb;
    }

    public void OfferRefused(Vector3 position, Action<AI> cb)
    {
        CurrentTime = 1;
        Delay = 0;
        TargetPosition = new Vector3(position.x, transform.position.y, position.z);
        Action = AI_Actions.GO_TO_POS_CB;
        ActionCb = cb;
    }

    public void GoToEntrance(Vector3 position)
    {
        TargetPosition = position;
        Action = AI_Actions.GO_TO_ENTRANCE;
        Animator.SetBool("Walk", true);
    }

    public void OnRespawn()
    {
        // TO DO 
    }

    public void Update()
    {

        switch (Action)
        {
            case AI_Actions.GO_TO_POS:
                GoToNextPositionUpdate();
                break;
            case AI_Actions.GO_TO_ENTRANCE:
                GoToEntranceUpdate();
                break;
            case AI_Actions.GO_TO_POS_CB:
                GoToPosCb();
                break;
        }
        // TODO
    }

    private void GoToPosCb()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime < Delay) return;

        Animator.SetBool("Walk", true);

        transform.position = transform.position + transform.forward * Speed * Time.deltaTime;
        transform.rotation = Utils.LookAt(transform, TargetPosition, 1f);

        if (Vector3.Distance(transform.position, TargetPosition) < 1f)
        {
            GoToIdle();
            ActionCb?.Invoke(this);
        }
    }

    private void GoToNextPositionUpdate()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime < Delay) return;

        Animator.SetBool("Walk", true);

        transform.position = transform.position + transform.forward * Speed * Time.deltaTime;

        if (transform.position.z < TargetPosition.z)
        {
            Animator.SetBool("Walk", false);
            GoToIdle();
            GameController.Instance.DispatchEvent("OnAiReachNextPosition");
        }
    }

    private void GoToIdle()
    {
        CurrentTime = 0f;
        Action = AI_Actions.IDLE;
        Animator.SetTrigger("Idle");
    }

    private void GoToEntranceUpdate()
    {

        transform.position = transform.position + transform.forward * Speed * Time.deltaTime;

        if (transform.position.z < TargetPosition.z)
        {
            Debug.Log("OnAiReachEntrance");
            Animator.SetBool("Walk", false);
            Action = AI_Actions.IDLE;
            Animator.SetTrigger("Talk");
            GameController.Instance.DispatchEvent("OnAiReachEntrance");
        }
    }

    private void OnDrawGizmos()
    {
        if (TargetPosition != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + transform.up, TargetPosition + transform.up);
        }
    }
}
public enum AI_Actions { IDLE, GO_TO_POS, GO_TO_POS_CB, GO_TO_ENTRANCE, EXIT, ENTER }
public enum Gender { MALE, FEMALE }