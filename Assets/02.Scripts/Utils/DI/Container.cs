using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoodyGo.Utils.DI
{
    public class Container
    {
        public Container()
        {
            _registration = new Dictionary<Type, object>();
        }

        Dictionary<Type, object> _registration;

        /// <summary>
        /// 생성자가 있는 일반 C# 클래스 등록 하는 함수 (생성해서 추가함)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>() where T : class, new() // T는 클래스이면서 생성자가 있는 얘 라고 제한을 걸어둠
        {
            T obj = new T();
            _registration[typeof(T)] = obj;
        } 

        /// <summary>
        /// Monobehaviour 객체를 생성해서 추가
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterMonobehaviour<T>() where T : MonoBehaviour
        {
            T obj = new GameObject(typeof(T).Name).AddComponent<T>();
            _registration[typeof(T)] = obj;
        }

        /// <summary>
        /// Hierarchy 에 존재하는 객체를 추가
        /// </summary>
        /// <param name="monobehaviour"></param>
        public void RegisterMonobehaviour(MonoBehaviour monobehaviour)
        {
            _registration[monobehaviour.GetType()] = monobehaviour;
        }

        /// <summary>
        /// 등록된 객체를 가져올 때 사용하는 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)_registration[typeof(T)];
        }

        public object Resolve(Type type)
        {
            if (_registration.TryGetValue(type, out object obj)) return obj;
            else return null;
        }
    }
}
