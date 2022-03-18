using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 10)
        {
            // Pooling할 Object의 원본(Original)과 Root를 설정한다.
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            // count 만큼 Object의 Poolable Component를 생성하여 Pooling Stack에 Push한다.
            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            // (Original)오브젝트에 Poolable Component 생성후 반환
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
            
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root; // pooling object(Root)를 parent로 설정
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            // Pool Stack에 반납한다. 반납 이전에 Pooling Info를 초기화해줌
            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            // Pool에서 꺼내어 사용함.

            if (_poolStack.Count > 0) // 사용하지 않는 pool이 있다면 꺼냄
                poolable = _poolStack.Pop();
            else // 없으면 Pool 추가생성
                poolable = Create();

            poolable.gameObject.SetActive(true);

            /* DontDestroyOnLoad 해제 용도
             * ex) Pool을 Default count 5로 초기화 -> 씬에서 10마리 생성
             *     -> 기존에 5는 DontDestroyOnLoad 산하로 들어가고 -> 나머지 5는 Init이 호출되지않아 DontDestroyOnLoad 밖에 생성된다. 
             * parant가 null이면 poolalble parent를 DontDestroyOnLoad밖의 hierarchy(Scene,Cam등)으로 설정해주어 DontDestroyOnLoad를 빠져나가게 한다.
             */
            if (parent == null) 
                poolable.transform.parent = Managers.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        // Pool 최상위 Root 설정
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 10)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root; // 최상단 @Pool_Root를 Parent로 설정

        _pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        
        if(_pool.ContainsKey(name) == false)
        {
            // 풀에 키가 없으면 (풀링X) 삭제
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        // 사용이 끝난후 풀에 다시 반환한다
        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original); //꺼내올수 있는 Pool이 없다면 생성

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        // 원본을 반환한다.
        return _pool[name].Original; 
    }

    public void Clear()
    {
        // @Pool_Root 산하의 child pool들을 전부 날려준다.

        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
