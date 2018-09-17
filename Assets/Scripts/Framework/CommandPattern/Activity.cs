using UnityEngine;
using UnityEngine.EventSystems;

public class Activity
{
    private GameObject m_player;            //玩家
    //private GameObject m_camera;
    public virtual ICommand ActivityHandle()
    {
        return null;
    }

    public Activity(GameObject player, GameObject camera)
    {
        m_player = player;
        //m_camera = camera;
    }

    public ICommand OnClickEvent(Vector3 clickPosition)
    {
        //Debug.Log("clickPosition:"+ clickPosition);
#if IPHONE || ANDROID
	if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#else
        if (!EventSystem.current.IsPointerOverGameObject())
#endif
        {
            //Debug.Log("1");
            Ray ray = Camera.main.ScreenPointToRay(clickPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<RMoveBaseRole>())
                    return new PathfindingCommand(m_player, hit.collider.gameObject);
                else if (hit.collider.GetComponent<MusicNote>())
                    return new MusicNoteCommand(hit.collider.gameObject);
                else if (hit.collider.GetComponent<DialogueNote>())
                    return new DialogueCommand(hit.collider.gameObject);
            }
            else
            {
                Vector2Int gezi = TileManager.Instance.ScreenPointToTilePoint(clickPosition);
                if (!TileManager.Instance.IsWallByGrid(gezi))
                {
                    CreateNewRipple(TileManager.Instance.ScreenPointToWorldPosition(clickPosition));
                }
                return new PathfindingCommand(m_player, TileManager.Instance.ScreenPointToTilePoint(clickPosition));
            }
        }
        else
        {

            Debug.Log("Click on the ui");
        }

        return null;
    }

    /// <summary>
    /// 创建新的波纹
    /// </summary>
    /// <param name="pos"></param>
    private void CreateNewRipple(Vector3 pos)
    {
        pos.z = 0;
        GameObject tempNewRipple = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/" + "RippleEffect"));
        tempNewRipple.transform.position = pos;
    }

    // 单手指滑动
    public ICommand OnMoveEvent(Vector3 vec3LastPostion, Vector3 vec3NowPosition)
    {
        Vector3 offset = vec3NowPosition - vec3LastPostion;

        return new CameraMoveCommand(offset);
    }

    // 两手指放大屏幕
    public ICommand OnFingerZoomInEvent()
    {
        Vector3 offset = Vector3.zero;
        return new CameraZoomInCommand(offset);
    }

    // 双手指缩小屏幕
    public ICommand OnFingerZoomOutEvent()
    {
        Vector3 offset = Vector3.zero;
        return new CameraZoomOutCommand(offset);
    }
}

