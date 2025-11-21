using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace LazyCoder.Gui
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class GuiOutline : MaskableGraphic
    {
        [Title("Config")]
        [SerializeField] Texture _texture;

        [SerializeField, Range(0f, 500f)] float _width = 10f;

        [SerializeField, Range(0f, 500f)] float _cornerRadius = 0f;

        [SerializeField, Range(0, 20)] int _cornerSegments = 0;

        [SerializeField, Range(0f, 1f)] float _mappingBias = 0.5f;

        [SerializeField] bool _fillCenter = false;

        private readonly Vector3[] _corners = new Vector3[4];

        private readonly List<UIVertex> _verts = new();

        public override Texture mainTexture => _texture == null ? s_WhiteTexture : _texture;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            SetVerticesDirty();
            SetMaterialDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            // Clamp corner radius
            var rect = rectTransform.rect;
            var clampedCornerRadius = Mathf.Min((Mathf.Min(rect.width, rect.height) / 2f), _cornerRadius);

            // If cornerSegments is zero, draw a squared-corner outline (outer rect expanded outside and inner rect = rect)
            if (_cornerSegments == 0)
            {
                _verts.Clear();

                // Get base rectangle corners (local)
                var baseCorners = new Vector3[4];
                rectTransform.GetLocalCorners(baseCorners); // order: bottom-left, top-left, top-right, bottom-right

                // Dimensions of the inner (original) rect
                var width = rect.width;
                var height = rect.height;

                // Outer rectangle is expanded outward by _outlineWidth on all sides
                var outerWidth = width + 2f * _width;
                var outerHeight = height + 2f * _width;

                var edgeLengths = new[] { outerHeight, outerWidth, outerHeight, outerWidth };
                var around = 2f * (outerWidth + outerHeight);
                if (around <= Mathf.Epsilon)
                    return;

                // Build verts in same winding/ordering: for each corner: inner, outer (, center if _fillCenter)
                var u = 0f;
                var vert = new UIVertex { color = color };
                for (var c = 0; c < 4; c++)
                {
                    var inner = baseCorners[c];
                    var outer = inner + new Vector3(Mathf.Sign(inner.x) * _width, Mathf.Sign(inner.y) * _width, 0f);

                    vert.position = inner;
                    vert.uv0 = new Vector2(u, 0f);
                    _verts.Add(vert);

                    vert.position = outer;
                    vert.uv0 = new Vector2(u, 1f);
                    _verts.Add(vert);

                    if (_fillCenter)
                    {
                        vert.position = rect.center;
                        vert.uv0 = new Vector2(u, 0f);
                        _verts.Add(vert);
                    }

                    u += edgeLengths[c] / around;
                }

                // Add end verts (u = 1)
                var firstInner = _verts[0];
                firstInner.uv0 = new Vector2(1f, 0f);
                _verts.Add(firstInner);

                var firstOuter = _verts[1];
                firstOuter.uv0 = new Vector2(1f, 1f);
                _verts.Add(firstOuter);

                if (_fillCenter)
                {
                    var firstCenter = _verts[2];
                    firstCenter.uv0 = new Vector2(1f, 0f);
                    _verts.Add(firstCenter);
                }

                // Add verts to VertexHelper
                foreach (var v in _verts)
                    vh.AddVert(v);

                // Add triangles (reuse existing logic)
                if (_fillCenter)
                {
                    for (var v = 0; v < vh.currentVertCount - 3; v += 3)
                    {
                        vh.AddTriangle(v, v + 1, v + 4);
                        vh.AddTriangle(v, v + 4, v + 3);

                        vh.AddTriangle(v + 2, v, v + 3);
                        vh.AddTriangle(v + 2, v + 3, v + 5);
                    }
                }
                else
                {
                    for (var v = 0; v < vh.currentVertCount - 2; v += 2)
                    {
                        vh.AddTriangle(v, v + 1, v + 3);
                        vh.AddTriangle(v + 3, v + 2, v);
                    }
                }

                return;
            }

            // Offset corner based on clamped corner radius (rounded corners path)
            rectTransform.GetLocalCorners(_corners);
            _corners[0] += new Vector3(clampedCornerRadius, clampedCornerRadius, 0f);
            _corners[1] += new Vector3(clampedCornerRadius, -clampedCornerRadius, 0f);
            _corners[2] += new Vector3(-clampedCornerRadius, -clampedCornerRadius, 0f);
            _corners[3] += new Vector3(-clampedCornerRadius, clampedCornerRadius, 0f);

            // Calculate dimensions
            var h = _corners[1].y - _corners[0].y;
            var w = _corners[2].x - _corners[1].x;
            var edgeLens = new[] { h, w, h, w };
            var circumference =
                2f * Mathf.PI * Mathf.Lerp(clampedCornerRadius, clampedCornerRadius + _width, _mappingBias);
            var aroundTotal = h * 2f + w * 2f + circumference;
            var cornerLength = circumference / 4f;
            var segmentLength = cornerLength / _cornerSegments;

            var vertex = new UIVertex { color = color };
            _verts.Clear();

            // Create corners (rounded)
            var uu = 0f;
            for (var c = 0; c < 4; c++)
            {
                var origin = _corners[c];
                for (var i = 0; i < _cornerSegments + 1; i++)
                {
                    var angle = (float)i / _cornerSegments * Mathf.PI / 2f + Mathf.PI * 0.5f - Mathf.PI * c * 1.5f;
                    var direction = new Vector3(Mathf.Cos(-angle), Mathf.Sin(-angle), 0f);

                    vertex.position = origin + direction * clampedCornerRadius;
                    vertex.uv0 = new Vector2(uu, 0f);
                    _verts.Add(vertex);

                    vertex.position = origin + direction * (clampedCornerRadius + _width);
                    vertex.uv0 = new Vector2(uu, 1f);
                    _verts.Add(vertex);

                    if (_fillCenter)
                    {
                        vertex.position = rect.center;
                        vertex.uv0 = new Vector2(uu, 0f);
                        _verts.Add(vertex);
                    }

                    if (i < _cornerSegments)
                        uu += segmentLength / aroundTotal;
                    else
                        uu += edgeLens[c] / aroundTotal;
                }
            }

            // Add end verts
            vertex = _verts[0];
            vertex.uv0 = new Vector2(1f, 0f);
            _verts.Add(vertex);

            vertex = _verts[1];
            vertex.uv0 = new Vector2(1f, 1f);
            _verts.Add(vertex);

            if (_fillCenter)
            {
                vertex = _verts[2];
                vertex.uv0 = new Vector2(1f, 0f);
                _verts.Add(vertex);
            }

            // Add verts to VertexHelper
            foreach (var vtx in _verts)
                vh.AddVert(vtx);

            // Add triangles to VertexHelper
            if (_fillCenter)
            {
                for (var v = 0; v < vh.currentVertCount - 3; v += 3)
                {
                    vh.AddTriangle(v, v + 1, v + 4);
                    vh.AddTriangle(v + 4, v + 3, v);

                    vh.AddTriangle(v + 2, v, v + 3);
                    vh.AddTriangle(v + 2, v + 3, v + 5);
                }
            }
            else
            {
                for (var v = 0; v < vh.currentVertCount - 2; v += 2)
                {
                    vh.AddTriangle(v, v + 1, v + 3);
                    vh.AddTriangle(v + 3, v + 2, v);
                }
            }
        }
    }
}