using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{ 
   [Header("Components")]
   [SerializeField] private SpriteRenderer _renderer;
   [SerializeField] private BoxCollider2D _collider;
   [SerializeField] private GameObject _cross;
   
   [Header("Settings")]
   [SerializeField] private Color _baseColor, offsetColor;
   [SerializeField] private bool isCrossed;

   public bool IsCrossed => isCrossed;
   private Material _dissolveMat;
   private readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

   #region UNITY EVENTS

   private void Start()
   {
       _dissolveMat = _cross.GetComponent<SpriteRenderer>().material;
   }
   
   private void OnMouseDown()
   {
       if (isCrossed) return;
       
       _collider.enabled = false;
       isCrossed = true;
       _cross.SetActive(true);
       GameManager.Instance.OnTileCrossed(this);
   }

   #endregion

   #region PUBLIC METHODS

   public void Init(bool isOffset)
   {
       _renderer.color = isOffset ? offsetColor : _baseColor;
   }

   public void OnReset()
   {
       isCrossed = false;
       DOTween.To(() => _dissolveMat.GetFloat(_dissolveAmount), 
           x => _dissolveMat.SetFloat(_dissolveAmount, x), 0, 0.7f).OnComplete(() =>
       {
           _cross.SetActive(false);
           _dissolveMat.SetFloat(_dissolveAmount, 1);
           _collider.enabled = true;
       });
   }

   #endregion
}
