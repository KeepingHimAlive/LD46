using System.Collections.Generic;
using System.Linq;
using SaveBilly;
using UnityEngine;
using UnityEngine.AI;

public class BillyController : MonoBehaviour
{
   public static BillyController instance;

   private NavMeshAgent _agent;

   [SerializeField] private LayerMask _foodMask;

   public List<FoodBase> activelyEating = new List<FoodBase>();

   private Transform TargetItem;

   public Transform mouthTransform;

   private Animator anim;

   [Range(0, 100)] public float health = 100;
   [Range(0, 100)] public float sugar = 50;
   [Range(0, 100)] public float hunger = 50;
   [Range(0, 100)] public float thirst = 50;


   public UIFillBar healthBar;
   public UIFillBar sugarBar;
   public UIFillBar hungerBar;
   public UIFillBar thirstBar;

   public float foodConsumeRate = 1;
   private static readonly int Speed_AnimParam = Animator.StringToHash("Speed");
   private static readonly int Health_AnimParam = Animator.StringToHash("Health");

   private float searchTimer = 2;
   private Vector3[] _corners = new Vector3[100];
   private bool _isHungry;

   private void OnEnable()
   {
      instance = this;
   }

   void Start()
   {
      _agent = GetComponent<NavMeshAgent>();
      anim = GetComponent<Animator>();
   }

   private void Update()
   {
      var overlapped = Physics.OverlapSphere(transform.position, 20, _foodMask.value);

      // Debug.Log(string.Join(" - ", colliders.ToList()));

      var diff = hunger - thirst;
      _isHungry = diff > 0;

      //if he has no targe, set one
      if (TargetItem == null)
      {
         SeekNearestTarget(overlapped, _isHungry);
      }

      searchTimer -= Time.deltaTime;
      if (searchTimer < 0)
      {
         searchTimer += 2;
         SeekNearestTarget(overlapped, _isHungry);
      }

      //nom nom
      for (int i = 0; i < activelyEating.Count; i++)
      {
         var food = activelyEating[i];
         if (food == null) continue;
         food.EatFood(foodConsumeRate);

         switch (food.foodType)
         {
            case FoodType.GoodFood:
               hunger -= foodConsumeRate * 3 * Time.deltaTime;
               break;

            case FoodType.GoodDrink:
               thirst -= foodConsumeRate * 3 * Time.deltaTime;
               if (thirst < 0) thirst = 0;
               break;

            case FoodType.BadFood:
               hunger -= foodConsumeRate * 2 * Time.deltaTime;
               sugar += foodConsumeRate * 2 * Time.deltaTime;
               break;

            case FoodType.BadDrink:
               thirst -= foodConsumeRate * 2 * Time.deltaTime;
               sugar += foodConsumeRate * 2 * Time.deltaTime;
               if (thirst < 0) thirst = 0;
               break;
         }
      }

      thirst += Time.deltaTime * foodConsumeRate * .3f;
      hunger += Time.deltaTime * foodConsumeRate * .3f;

      if (thirst > 90 || hunger > 90 || sugar > 90)
      {
         health -= Time.deltaTime * 10f;
      }

      sugar -= Time.deltaTime * foodConsumeRate * .125f;

      sugar = Mathf.Clamp(sugar, 0, 100);
      thirst = Mathf.Clamp(thirst, 0, 100);
      hunger = Mathf.Clamp(hunger, 0, 100);

      healthBar.SetValue(health);
      sugarBar.SetValue(sugar);
      hungerBar.SetValue(hunger);
      thirstBar.SetValue(thirst);

      if (health < 0)
      {
         LevelControler.Instance.GameOver();
      }

      anim.SetFloat(Speed_AnimParam, _agent.velocity.magnitude);
      anim.SetFloat(Health_AnimParam, health);
   }

   private void SeekNearestTarget(Collider[] overlapped, bool isHungry)
   {
      //selecting drink items or food items
      var overlappedFood = overlapped.Where(collider1 =>
      {
         var food = collider1.GetComponentInParent<FoodBase>();
         var isFood = food.foodType == FoodType.GoodFood || food.foodType == FoodType.BadFood;
         return isFood == isHungry;
      }).ToList();

      //if it didnt find something he's craving, compromise 
      if (overlappedFood.Count == 0) overlappedFood = overlapped.ToList();

      //var nearestFood = GetNearest(overlappedFood);

      var path = new NavMeshPath();
      var orderByDistance = overlappedFood.OrderBy(c =>
      {
         double d = 0;
         if (_agent.CalculatePath(c.transform.position, path))
         {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
               for (var i = 0; i < path.corners.Length - 1; i++)
               {
                  d += Vector3.Magnitude(path.corners[i] - path.corners[i + 1]);;
               }
            }
            else
            {
               d += Vector3.Magnitude(c.transform.position - transform.position) + 100;
            }
         }
         
         return d;
      }).ToList();

      
      if (orderByDistance.Count > 0)
      {
         TargetItem = orderByDistance[0].transform;
         _agent.SetDestination(orderByDistance[0].transform.position);
      }
      
      // Debug.DrawLine(transform.position, orderByDistance[0].transform.position, Color.red, 1f);
      // for (var i = 0; i < orderByDistance.Count -1 ; i++)
      // {
      //     Debug.DrawLine(orderByDistance[i].transform.position, orderByDistance[i + 1].transform.position, Color.red, 1f);
      // }

      // for (var i = 0; i < path.corners.Length -1; i++)
      // {
      //    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 1f );
      //    
      // }
   }

   private void OnTriggerEnter(Collider other)
   {
      var food = other.gameObject.GetComponentInParent<FoodBase>();
      if (food != null)
      {
         activelyEating.Add(food);
         //_agent.isStopped = true;
         food.OnFinishedHandler += () =>
         {
            activelyEating.Remove(food);
            Destroy(food.gameObject);
            //_agent.isStopped = false;
         };
      }
   }

   private void OnTriggerExit(Collider other)
   {
      var food = other.gameObject.GetComponentInParent<FoodBase>();
      if (food != null) activelyEating.Remove(food);
   }

   public void PlayHopSound()
   {
      var audioSource = GetComponent<AudioSource>();
      audioSource.Play();
   }

   // private void OnDrawGizmos()
   // {
   //    // if (Application.isPlaying)
   //    // {
   //    //    Handles.Label(transform.position, _agent.path.status.ToString());
   //    // }
   // }
}