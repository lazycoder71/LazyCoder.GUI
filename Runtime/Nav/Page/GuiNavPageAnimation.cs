using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LazyCoder.AnimationSequencer;
using UnityEngine;

namespace LazyCoder.Gui
{
    [System.Serializable]
    public class GuiNavPageAnimation
    {
        [SerializeField] private float _duration;

        [SerializeField] private AnimationSequence _animationSequence;

        [SerializeField] private bool _isBackward;

        public async UniTask Play(CancellationToken cancellationToken)
        {
            // If no animation sequence or duration is zero or negative, skip animation
            if (_animationSequence == null || _duration <= 0f)
                return;

            // Set sequence's timescale to match duration
            _animationSequence.Sequence.timeScale = _animationSequence.Sequence.Duration() / _duration;

            // Play the sequence based on direction
            if (_isBackward)
            {
                _animationSequence.Sequence.Complete();
                _animationSequence.Sequence.PlayBackwards();
            }
            else
            {
                _animationSequence.Sequence.Restart();
                _animationSequence.Sequence.Play();
            }

            // Await for duration
            await UniTask.WaitForSeconds(_duration, true, cancellationToken: cancellationToken);
        }

        public void Stop()
        {
            // If no animation sequence or duration is zero or negative, skip animation
            if (_animationSequence == null || _duration <= 0f)
                return;

            // Move the sequence to the end based on direction
            if (_isBackward)
                _animationSequence.Sequence.Restart();
            else
                _animationSequence.Sequence.Complete();

            _animationSequence.Stop();
        }
    }
}