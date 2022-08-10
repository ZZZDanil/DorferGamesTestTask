using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBagStack : MonoBehaviour
{

    public delegate void PlayerBagStackHandler(PlayerBagStack message);
    public event PlayerBagStackHandler changeBag;

    /*Gizmo*/
    public float boxWidth = 5;
    public float boxHeight = 5;
    public float boxSize = 5;
    /*Cells*/
    public int width = 5;
    public int height = 5;
    public int size = 5;

    public int maxCount = 1;
    public int count = 0;

    private List<GameObject> bag;
    private Vector3 startBoxPos;

    private float widthStep = 1;
    private float heightStep = 1;
    private float upStep = 1;


    private void Awake()
    {
        bag = new List<GameObject>();
        widthStep = boxWidth / width;
        heightStep = boxHeight / height;
        upStep = boxSize / size;
        startBoxPos = new Vector3((boxWidth - widthStep) / 2, 0, (boxHeight - heightStep) / 2);
    }
    private void Start()
    {
        maxCount = StaticData.settings.playerBagSize;
        changeBag += StaticData.UIBag.UpdateViewByPlayerBagStack;
        changeBag(this);
    }
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Vector3 pos = transform.position;
        Quaternion quaternion = transform.rotation;

        Vector3[] lineSegments = new Vector3[24]{
            pos +  quaternion * (new Vector3(boxWidth/2, 0, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, 0, boxHeight/2)),

            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, -boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, boxHeight/2)),


            pos + quaternion * (new Vector3(boxWidth/2, 0, boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(-boxWidth/2, boxSize/2, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, 0, -boxHeight/2)),
            pos + quaternion * (new Vector3(boxWidth/2, boxSize/2, -boxHeight/2))

        };
        //Handles.DrawWireCube(playerBagStack.transform.position, myObj.size);

        Handles.DrawLines(lineSegments);
    }
#endif
    public void AddRes(GameObject res)
    {

        res.transform.parent = transform;
        bag.Add(res);
        AddAnimation(res, GlobalTargetBagPosition(count));
        res.transform.rotation = transform.rotation;
        count++;
        changeBag(this);
    }
    public GameObject RemoveRes(int pos)
    {
        GameObject obj = bag[pos];
        count--;
        bag.RemoveAt(pos);
        changeBag(this);
        return obj;
    }
    /* *** */
    private Vector3 GlobalTargetBagPosition(int bagResId)
    {
        return transform.position + transform.rotation * TargetBagPosition(bagResId);
    }
    private Vector3 TargetBagPosition(int bagResId)
    {
        Vector3 cellPos = new Vector3(
            (bagResId % width) * widthStep,
            (bagResId / (width * height)) * upStep,
            ((bagResId % (width * height)) / width) * heightStep);
        return cellPos - startBoxPos;
    }

    private void AddAnimation(GameObject res, Vector3 target)
    {
        float time = StaticData.settings.resourceGettingTime;

        int cell = count;
        Transform resTransform = res.transform;
        resTransform
            .DOMove((target + resTransform.position)/2 + new Vector3(0,1,0), time/2)
            .OnComplete(() => {
                Tweener tweener = resTransform.DOMove(target, time/2);
                StartCoroutine(CorrectingNavigationToTarget(resTransform, tweener, cell));
            });
        resTransform.DOScale(StaticData.settings.playerBagResourceScaler, time);

    }
    private IEnumerator CorrectingNavigationToTarget(Transform gameObjectTransform, Tweener t, int bagResId)
    {
        float time = 0;
        while (t.IsActive() && t.IsPlaying() == true)
        {
            Vector3 pos = GlobalTargetBagPosition(bagResId);
            time = t.Elapsed(true);
            t.ChangeEndValue(pos, false);
            t.Goto(time, true);
            yield return new WaitForSeconds(0.05f);
        }
        gameObjectTransform.position = GlobalTargetBagPosition(bagResId);
        yield return null;
    }
}
