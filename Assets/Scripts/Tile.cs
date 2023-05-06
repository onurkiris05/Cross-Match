using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{ 
   [Header("Components")]
   [SerializeField] private SpriteRenderer _renderer;
   [SerializeField] private BoxCollider2D _collider;
   [SerializeField] private GameObject _cross;
   
   [Space][Header("Settings")]
   [SerializeField] private Color _baseColor;
   [SerializeField] private Color _offsetColor;
   [SerializeField] private bool _isCrossed;

   public bool IsCrossed => _isCrossed;
   private Material _dissolveMat;
   private readonly int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");

   #region UNITY EVENTS

   private void Start()
   {
       _dissolveMat = _cross.GetComponent<SpriteRenderer>().material;
   }
   
   private void OnMouseDown()
   {
       if (_isCrossed || EventSystem.current.IsPointerOverGameObject()) return;
       
       _collider.enabled = false;
       _isCrossed = true;
       _cross.SetActive(true);
       GameManager.Instance.OnTileCrossed(this);
   }

   #endregion

   #region PUBLIC METHODS

   public void Init(bool isOffset)
   {
       _renderer.color = isOffset ? _offsetColor : _baseColor;
   }

   public void OnReset()
   {
       _isCrossed = false;
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
