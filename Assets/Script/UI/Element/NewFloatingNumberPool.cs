using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{

    public class NewFloatingNumberPool : MonoBehaviour
    {
        public NewFloatingNumber FloatingNumber;
        public float NextTime; //每幾秒生一個
        public float ShowTime; //顯示幾秒

        private Transform _anchor;
        private Timer _showTimer = new Timer();
        private Queue<NewFloatingNumber> _objectPool = new Queue<NewFloatingNumber>();
        private Queue<FloatingNumberData> _dataQueue = new Queue<FloatingNumberData>();

        public void SetAnchor(Transform anchor)
        {
            _anchor = anchor;
        }

        public void Play(List<FloatingNumberData> list)
        {
            _dataQueue = new Queue<FloatingNumberData>(list);
            Play();
        }

        private void Play()
        {
            NewFloatingNumber floatingNumber = _objectPool.Dequeue();
            floatingNumber.Play(ShowTime, transform.position, _dataQueue.Dequeue());
            _showTimer.Start(NextTime, () => //顯示下一個數字
            {
                if (_dataQueue.Count > 0)
                {
                    Play();
                }
            });
        }

        private void Recycle(NewFloatingNumber floatingNumber)
        {
            _objectPool.Enqueue(floatingNumber);
        }

        private void InitObjectPool()
        {
            int count = Mathf.CeilToInt(ShowTime / NextTime);
            NewFloatingNumber floatingNumber;
            for (int i = 0; i < count; i++)
            {
                floatingNumber = Instantiate(FloatingNumber);
                floatingNumber.transform.SetParent(transform);
                floatingNumber.RecycleHandler += Recycle;
                _objectPool.Enqueue(floatingNumber);
            }
        }

        private void Awake()
        {
            InitObjectPool();
        }

        void Update()
        {
            if (_anchor != null)
            {
                this.transform.position = Camera.main.WorldToScreenPoint(_anchor.position);
            }
        }
    }
}