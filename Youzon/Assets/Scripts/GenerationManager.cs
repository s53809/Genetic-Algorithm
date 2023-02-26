using System.Collections.Generic;
using UnityEngine;

public enum agentAction
{
    Up,
    Down,
    Left,
    Right,
}
public class GenerationManager : MonoBehaviour
{
    [Header("Initialization")]
    [SerializeField] private GameObject _agentPrefabs;
    [SerializeField] private GameObject _startPos;

    [Header("Settings")] 
    [SerializeField] private int _agentNum;
    [SerializeField] private float _mutationPercentage;
    [SerializeField] private float _stateTime;
    [SerializeField] private int _actionNum;
    [SerializeField] private float _moveSpeed;

    private int _geneNum;
    private List<EntityController> _entityActions;
    private float _lastGeneration = 0;
    
    private void Awake()
    {
        _entityActions  = new List<EntityController>();
    }

    private void Start()
    {
        Invoke("Init", 2.0f);
    }

    private void Init()
    {
        _geneNum = 0;
        _lastGeneration = Time.time;
        UIManagement.ViewGene(_geneNum);
        for (int i = 0; i < _agentNum; i++)
        {
            GameObject obj = Instantiate(_agentPrefabs);
            obj.transform.position = _startPos.transform.position;
            EntityController ec = obj.GetComponent<EntityController>();

            agentAction[] actions = new agentAction[_actionNum];
            for (int j = 0; j < _actionNum; j++)
            {
                actions[j] = (agentAction)UnityEngine.Random.Range(0, 4);
            }
            
            ec.SetAction(actions);
            ec.moveSpeed = _moveSpeed;
            ec.moveCoolTime = _stateTime;
            _entityActions.Add(ec);
        }
    }

    private int Select()
    {
        int pick = UnityEngine.Random.Range(0, 101);

        int selectedNum = -1;
        if (0 <= pick && pick <= 40) selectedNum = 0;
        else if (41 <= pick && pick <= 70) selectedNum = 1;
        else if (71 <= pick && pick <= 90) selectedNum = 2;
        else if (91 <= pick && pick <= 100) selectedNum = 3;

        return selectedNum;
    }

    private void DestroyObj()
    {
        for (int i = 0; i < _entityActions.Count; i++)
        {
            Destroy(_entityActions[i].gameObject);
        }
        _entityActions.Clear();
    }

    private void Crossover()
    {
        _entityActions.Sort();

        List<agentAction[]> newChilds = new List<agentAction[]>();

        for (int i = 0; i < _agentNum - 1; i++)
        {
            int firstGene = Select();
            int secondGene = Select();
            while (secondGene == firstGene) secondGene = Select();

            int[] mask = new int[_actionNum];
            for (int j = 0; j < _actionNum; j++)
            {
                mask[j] = UnityEngine.Random.Range(0, 2);
            }

            agentAction[] babyGene = new agentAction[_actionNum];
            
            for (int j = 0; j < _actionNum; j++)
            {
                if (mask[j] == 0) babyGene[j] = _entityActions[firstGene].Actions[j];
                else babyGene[j] = _entityActions[secondGene].Actions[j];

                int mutation = UnityEngine.Random.Range(0, 101);
                if (mutation <= _mutationPercentage) babyGene[j] = (agentAction)UnityEngine.Random.Range(0, 4);
            }
            
            newChilds.Add(babyGene);
        }
        
        newChilds.Add(_entityActions[0].Actions);
        UIManagement.ViewResult(_entityActions[0]);
        
        DestroyObj();
        _geneNum++;
        
        for (int i = 0; i < _agentNum; i++)
        {
            GameObject obj = Instantiate(_agentPrefabs);
            obj.transform.position = _startPos.transform.position;
            EntityController ec = obj.GetComponent<EntityController>();

            ec.SetAction(newChilds[i]);
            ec.moveSpeed = _moveSpeed;
            ec.moveCoolTime = _stateTime;
            _entityActions.Add(ec);
        }
    }
    
    private void Update()
    {
        if (_lastGeneration + (_stateTime * (_actionNum + 1)) < Time.time)
        {
            _lastGeneration = Time.time;
            Crossover();
            UIManagement.ViewGene(_geneNum);
        }
    }
}