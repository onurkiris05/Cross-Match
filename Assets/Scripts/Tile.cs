using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{ 
   [Header("Components")]
   [SerializeField] private SpriteRenderer _renderer;
   [SerializeField] private GameObject _highlight, _cross;
   
   [Header("Settings")]
   [SerializeField] private Color _baseColor, offsetColor;


   public bool IsCrossed { get; private set; }
   
   public void Init(bool isOffset)
   {
       _renderer.color = isOffset ? offsetColor : _baseColor;
   }

   private void OnMouseDown()
   {
       IsCrossed = true;
       _cross.SetActive(true);
   }

   private void OnMouseEnter()
   {
       _highlight.SetActive(true);
   }

   private void OnMouseExit()
   {
       _highlight.SetActive(false);
   }
}
