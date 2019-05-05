using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    private GameObject _target;
    
    private NavMeshAgent _agent;
    public string targetTag;

    // Start is called before the first frame update
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        SelectTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameObject.activeSelf && (_target == null || !_target.activeSelf))
        {
            SelectTarget();
        }
    }

    private void SelectTarget()
    {
        var playerObjects = GameObject.FindGameObjectsWithTag(targetTag);

        var entries = playerObjects.ToList().Where(o => o != null && o.activeSelf).ToList();
        
        if (!entries.Any())
        {
            Destroy(gameObject);
            return;
        }
        
        _target = WeightedChoice.SelectFromList(
            entries,
            targetGameObject => 
                1 / Vector3.Distance(
                    targetGameObject.GetComponent<Transform>().position,
                    gameObject.GetComponent<Transform>().position
                )
        );

        _agent.SetDestination(_target.GetComponent<Transform>().position);
    }
}