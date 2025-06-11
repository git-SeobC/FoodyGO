using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace FoodyGo.Utils.DI
{
    public abstract class Scope : MonoBehaviour
    {
        protected Container container;

        [SerializeField] List<MonoBehaviour> _monobehaviours;

        protected virtual void Awake()
        {
            container = new Container();
            Register();
            InjectAll();
        }

        public virtual void Register()
        {
            foreach (var monoBehaviour in _monobehaviours)
            {
                container.RegisterMonobehaviour(monoBehaviour);
            }
        }

        protected virtual void InjectAll()
        {
            MonoBehaviour[] monobehaviours = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            foreach (var monoBehaviour in monobehaviours)
            {
                Inject(monoBehaviour);
            }
        }

        /// <summary>
        /// 의존성을 주입함
        /// </summary>
        /// <param name="target"> 주입할 대상 </param>
        protected virtual void Inject(object target)
        {
            Type type = target.GetType();

            // DeclaredOnly 옵션을 사용하여 현재 클래스에서 선언된 필드만 가져옴
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.GetCustomAttribute<InjectAttribute>() != null)
                {
                    //fieldInfo.GetType() 을 하면안됨, FieldType을 통해 FieldInfo의 원래 타입을 가지고 와야함
                    object value = container.Resolve(fieldInfo.FieldType);
                    fieldInfo.SetValue(target, value);
                }
            }
        }
    }
}
