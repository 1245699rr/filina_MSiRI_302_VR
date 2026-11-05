using UnityEngine;

public class Moh1 : MonoBehaviour
{

  // Массив названиями фигур
string[] figures = { "Cube_1", "Cube_2", "Cube_3"};

// Проверка:является ли объект созданным кубом 
public bool isGeneratedCube = false;
void Start()
{//Если это куб,не создаваем новые объекты 
     for (int i = 0; i < figures.Length; i++)
    {
       GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
       //Название из массива 
        cube.name = figures[i];

       //Расположение куба 
         cube.transform.position = new Vector3((i-1)*3,1,0);
       // Добавление физики 
       Rigidbody rb = cube.AddComponent<Rigidbody>();

       rb.useGravity = false;

    /*  rb.constraints = RigidbodyConstraints.FreezePositiony |
                       RigidbodyConstraints. FreezePositionz | 
                       RigidbodyConstraints. FreezeRotation;
    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;*/

// Движение кубов навстречу друг другу

    /*if (i == 0)
    {
        rb.linearVelocity = Vector3.right;

    }

    else if (i == 1)

    {

         rb.constraints | RigidbodyConstraints. FreezePositionx;

   }

   else

    {

           rb.linearVelocity = Vector3.left;

    }*/

// Добавление обработчика столкновений
//CubeManager handler = cube.AddComponent<CubeManager>();

//handler.isGeneratedCube = true;

  }

}

void Update()

{

}

// обработчик столкновений

    private void OnCollisionEnter (Collision collision)
    {
        if(collision.gameObject.name == "Cube"){
            collision.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
   


       


