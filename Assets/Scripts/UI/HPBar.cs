using UnityEngine;
using UnityEngine.UI;

namespace AnimarsCatcher
{
    public class HPBar : MonoBehaviour
    {
        private ReactiveProperty<int> mHP;
        private Image mHPBar;
        private int mHPMax;

        private void Awake()
        {
            mHPBar = GetComponent<Image>();
        }

        public void Init(ReactiveProperty<int> hp)
        {
            mHP = hp;
            mHP.Subscribe(OnHPChanged);
            mHPMax = mHP.Value;
        }

        private void OnHPChanged(int hp)
        {
            mHPBar.fillAmount = (float)hp / mHPMax;
            if (hp <= 0)
            {
                mHP.Unsubsribe(OnHPChanged);
                Destroy(transform.parent.parent.gameObject);
            }
        }
    }
}

