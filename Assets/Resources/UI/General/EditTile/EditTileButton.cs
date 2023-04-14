using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditTileButton : MyButton
{
    protected override void Update()
    {
        base.Update();
        //Debug.Log("pos:" + transform.position);
    }
    public void Appear(Vector3 startPos, Vector3 targetPos)
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = startPos;
        transform.position += new Vector3(0,0,-2f);
        animationBuffer.Add(new UpdatePosAnimatorInfo(gameObject, targetPos, true, 0.1f));
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Appear, 0.07f));
    }
    public void Disappear(Vector2 targetPos)
    {
        animationBuffer.Add(new UpdatePosAnimatorInfo(gameObject, targetPos, true, 0.1f));
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearDisappear, 0.07f));
        StartCoroutine(Delete());
    }
    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
