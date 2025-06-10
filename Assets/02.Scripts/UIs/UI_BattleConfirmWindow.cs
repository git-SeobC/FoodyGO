using FoodyGo.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace FoodyGo.UIs
{
    public class UI_BattleConfirmWindow : UI_Base
    {
        [SerializeField] private Button _confirm;
        [SerializeField] private Button _cancel;

        private void Start()
        {
            _confirm.onClick.AddListener(() =>
            {
                GameManager.instance.ActiveAdditiveScene("Catch");
                Hide();
            });

            _cancel.onClick.AddListener(() =>
            {
                Hide();
            });
        }
    }
}
