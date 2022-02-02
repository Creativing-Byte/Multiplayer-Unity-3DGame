using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Krivodeling.UI.Effects
{
    public class UIBlur : MonoBehaviour
    {
        public Color Color = Color.white;
        public float Intensity { get => _intensity; set => _intensity = Mathf.Clamp01(value); }
        public float Multiplier { get => _multiplier; set => _multiplier = Mathf.Clamp01(value); }

        public UnityEvent OnBeginBlur;
        public UnityEvent OnEndBlur;
        public BlurChangedEvent OnBlurChanged;

        [SerializeField, Range(0f, 1f)]
        private float _intensity;
        [SerializeField, Range(0f, 1f)]
        private float _multiplier = 0.15f;

        private Material _material;
        private int _colorId;
        private int _intensityId;
        private int _multiplierId;

        public void SetBlur(Color color, float intensity, float multiplier)
        {
            _material.SetColor(_colorId, color);
            _material.SetFloat(_intensityId, intensity);
            _material.SetFloat(_multiplierId, multiplier);
        }

        public void SetBlur(float value)
        {
            _material.SetFloat(_intensityId, value);
        }

        public void BeginBlur(float speed)
        {
            StopAllCoroutines();
            StartCoroutine(BeginBlurCoroutine(speed));
        }

        public void EndBlur(float speed)
        {
            StopAllCoroutines();
            StartCoroutine(EndBlurCoroutine(speed));
        }

        private void Start()
        {
            SetComponents();
            SetBlur(Color, Intensity, _multiplier);
        }

        private void SetComponents()
        {
            _material = FindMaterial();
            _colorId = Shader.PropertyToID("_Color");
            _intensityId = Shader.PropertyToID("_Intensity");
            _multiplierId = Shader.PropertyToID("_Multiplier");
        }

        private Material FindMaterial()
        {
            Material material = GetComponent<Image>().material;

            if (material == null)
                material = GetComponent<Renderer>().material;

            if (material == null)
                throw new NullReferenceException("Material not found");

            return material;
        }

        private IEnumerator BeginBlurCoroutine(float duration)
        {
            OnBeginBlur?.Invoke();

            while (Intensity < 1f)
            {
                Intensity += duration * Time.deltaTime;

                SetBlur(Intensity);

                OnBlurChanged.Invoke(Intensity);

                yield return null;
            }
        }

        private IEnumerator EndBlurCoroutine(float speed)
        {
            while (Intensity > 0f)
            {
                Intensity -= speed * Time.deltaTime;

                SetBlur(Intensity);

                OnBlurChanged.Invoke(Intensity);

                yield return null;
            }

            OnEndBlur?.Invoke();
        }

        [Serializable]
        public class BlurChangedEvent : UnityEvent<float> { }

        #region Editor
#if UNITY_EDITOR
        private void OnValidate()
        {
            SetBlurInEditor();
        }

        private void SetBlurInEditor()
        {
            Material material = FindMaterial();

            material.SetColor("_Color", Color);
            material.SetFloat("_Intensity", Intensity);
            material.SetFloat("_Multiplier", _multiplier);

            EditorUtility.SetDirty(material);
        }
#endif
        #endregion
    }
}
