using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandManager : MonoBehaviour {

    private bool isRun = true;
    Stack<ICommand> m_playerCommandStack;         //操作序列

    private float m_callbackTime;
    private GameObject m_Player;            //玩家

    private Activity m_activity;
    private void Start()
    {
        m_playerCommandStack = new Stack<ICommand>();
        m_callbackTime = 0;
        m_Player = GameObject.FindWithTag("Player");
        if (Input.touchSupported)
        {
            m_activity = new FingerActivity(m_Player, null);
        }
        else
        {
            m_activity = new MouseActivity(m_Player, null);
        }
    }

    private void Update()
    {
        if (isRun)
        {
            Control();
        }
        else
        {
            RunCallBack();
        }
    }

    /// <summary>
    /// 控制对象行动，添加命令到命令序列
    /// </summary>
    void Control()
    {
        m_callbackTime += Time.deltaTime;
        ICommand cmd = InputHandler();
        if(cmd != null)
        {
            m_playerCommandStack.Push(cmd);
            cmd.Execute();
        }
    }

    void RunCallBack()
    {
        m_callbackTime -= Time.deltaTime;
        if(m_callbackTime <= 0)
        {
            m_playerCommandStack.Pop().Undo();
        }
    }

    ICommand InputHandler()
    {
        if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow))
        {
            return new MoveCommand(m_Player, new Vector2Int(0, 1));
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            return new MoveCommand(m_Player, new Vector2Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return new MoveCommand(m_Player, new Vector2Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            return new MoveCommand(m_Player, new Vector2Int(1, 0));
        }
        //if(Input.GetMouseButtonDown(0))
        //{
        //    if(!EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);       
        //        RaycastHit hit;
        //        if(Physics.Raycast(ray, out hit))
        //        {
        //            return new PathfindingCommand(m_Player, hit.collider.gameObject);
        //        }
        //        else
        //        {
        //            return new PathfindingCommand(m_Player, TileManager.Instance.ScreenPointToTilePoint(Input.mousePosition));
        //        }
        //    }else
        //    {
        //        Debug.Log("Click on the ui");
        //    }

        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if(!EventSystem.current.IsPointerOverGameObject())
        //    {
        //        Debug.Log("hhh");
        //        Camera.main.GetComponent<CameraActivity>().Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.J))
        {
            //Vector3 vector3 = Vector3.zero;
            return new CameraZoomInCommand();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //Vector3 vector3 = Vector3.zero;
            return new CameraZoomOutCommand();
        }
        if (m_activity != null)
        {
            return m_activity.ActivityHandle();
        }
        return null;
    }

    //切换到回放模式
    public void Callback()
    {
        isRun = false;
    }

    //切换到运行模式
    public void Run()
    {
        isRun = true;
    }
}
