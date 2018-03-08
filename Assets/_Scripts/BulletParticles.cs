using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum COLOUR
{
    red,
    blue,
    green,
    purple
}

public class BulletParticles : MonoBehaviour
{
    public float emmiterFreqeuncy;
    public int maxEmmit;
    public float maxDur;
    public Vector3 random;
    public GameObject m_SpawnableObject;
    public Sprite[] m_Sprites;
    public COLOUR m_spriteColour;
    public float emissionSpeed;
    public bool Alive { get; set; }
    int size;



    #region dllImports
    [StructLayout(LayoutKind.Sequential)]
    public struct Vec
    {
        public float x, y, z;
    }
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        public Vec Pos;
        public Vec Vel;
        public float duration;
    }
    [DllImport("particle_bullet")]
    public static extern void initSystem(int maxEmmit, float maxDur, int maxArraySize, float emmiterFrequency);
    [DllImport("particle_bullet")]
    public static extern void initAllParticle(Particle[] p, float px, float py, float pz, float vx, float vy, float vz);
    [DllImport("particle_bullet")]
    public static extern void updateEmitterPos(float px, float py, float pz, float vx, float vy, float vz);
    [DllImport("particle_bullet")]
    public static extern void updateAllPartPos(Particle[] p, float dt, bool alive = true);
    [DllImport("particle_bullet")]
    public static extern void setRandomness(float rx, float ry, float rz);

    public void initParticles(Particle[] p, Vec pos, Vec vel)
    {
        initAllParticle(p, pos.x, pos.y, pos.z, vel.x, vel.y, vel.z);
    }
    public void updateEmmiter(Vec pos, Vec vel)
    {
        updateEmitterPos(pos.x, pos.y, pos.z, vel.x, vel.y, vel.z);
    }

    public void setRand(Vec rand)
    {
        setRandomness(rand.x, rand.y, rand.z);
    }
    #endregion

    Particle[] g_Arr;
    GameObject[] particleArr;

    public Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        Alive = true;
        //velocity = -GetComponent<Rigidbody>().velocity * emissionSpeed;
        size = (int)(maxEmmit * (maxDur) * (1 / emmiterFreqeuncy));
        g_Arr = new Particle[size];
        particleArr = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            particleArr[i] = Instantiate<GameObject>(m_SpawnableObject);
            particleArr[i].GetComponent<Renderer>().enabled = false;
            int rand = Random.Range(0, 3);
            if (rand == 1)
            {
                int temp = Random.Range((int)m_spriteColour * 3, (int)m_spriteColour * 3 + 3);
                particleArr[i].GetComponent<SpriteRenderer>().sprite = m_Sprites[temp];

            }
            else
            {
                int temp = Random.Range((12 + (int)m_spriteColour * 6), (12 + (int)m_spriteColour * 6 + 6));
                particleArr[i].GetComponent<SpriteRenderer>().sprite = m_Sprites[temp];
            }

        }
        //init statements
        initSystem(maxEmmit, maxDur, size, emmiterFreqeuncy);
        updateEmmiter(convert(transform.position), convert(velocity));
        initParticles(g_Arr, convert(transform.position), convert(velocity));
        setRand(convert(random));
    }


    Vec convert(Vector3 vector)
    {
        Vec temp;
        temp.x = vector.x;
        temp.y = vector.y;
        temp.z = vector.z;
        return temp;
    }
    void setPosition()
    {
        int count = 0;
        for (int i = 0; i < size; i++)
        {

            //if the particle isnt active disable the render
            if (g_Arr[i].duration == -10)
            {
                count++;
                particleArr[i].GetComponent<Renderer>().enabled = false;
            }
            //if it is active enable the render
            else
            {
                particleArr[i].GetComponent<Renderer>().enabled = true;
                particleArr[i].transform.position = new Vector3(g_Arr[i].Pos.x, g_Arr[i].Pos.y, g_Arr[i].Pos.z);
            }
        }
        if (count == size)
        {
            for (int i = 0; i < size; i++)
            {
                Destroy(particleArr[i]);
            }
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateEmmiter(convert(transform.position), convert(velocity));
        updateAllPartPos(g_Arr, Time.deltaTime, Alive);
        setPosition();

    }

    private void OnDestroy()
    {
        for (int i = 0; i < size; i++)
        {

            Destroy(particleArr[i]);
        }
    }
}