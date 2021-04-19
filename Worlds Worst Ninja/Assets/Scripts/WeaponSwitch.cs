using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public SpriteRenderer WeaponSprite;
    public List<Sprite> WeapSprites;
    public List<GameObject> Weapons,Colliders;

    private PlayerMovement _pm;
    private WeaponStat _WS;
    private int index;

    private Controls inputs;

    void Awake()
    {
        inputs = new Controls();
        inputs.Player.SwitchLeft.started+= context => SwitchLeft();
        inputs.Player.SwichRight.started += context => SwitchRight();
    }

        // Start is called before the first frame update
    void Start()
    {
        _pm = FindObjectOfType<PlayerMovement>();
        _WS = FindObjectOfType<WeaponStat>();
    }

    // Update is called once per frame
    void Update()
    {
        WeaponSprite.sprite = WeapSprites[index];
    }

    void SwitchLeft()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        index -= 1;
        if(index<0)
        {
            index = Weapons.Count - 1;
        }
        Weapons[index].SetActive(true);
        Colliders[index].SetActive(true);
    }

    void SwitchRight()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        index += 1;
        if (index > Weapons.Count - 1)
        {
            index = 0;
        }
        Weapons[index].SetActive(true);
        Colliders[index].SetActive(true);
    }

    public void Set1()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        StartCoroutine(_pm.Switch());
        index = 0;
        Weapons[0].SetActive(true);
        Colliders[0].SetActive(true);
    }

    public void Set2()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        StartCoroutine(_pm.Switch());
        index = 1;
        Weapons[1].SetActive(true);
        Colliders[1].SetActive(true);
    }

    public void Set3()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        StartCoroutine(_pm.Switch());
        index = 2;
        Weapons[2].SetActive(true);
        Colliders[2].SetActive(true);
    }

    public void Set4()
    {
        Weapons[index].SetActive(false);
        Colliders[index].SetActive(false);
        StartCoroutine(_pm.Switch());
        index = 3;
        Weapons[3].SetActive(true);
        Colliders[3].SetActive(true);
    }
    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
