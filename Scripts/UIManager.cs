using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public delegate void OnOpenUIDelegate(bool bSuccess, object param);
    public delegate void OnLoadUIItemDelegate(GameObject resItem, object param1);

    private Transform BaseUIRoot;      // λ��UI��ײ㣬��פ��������������
    private Transform PopUIRoot;       // λ��UI�ϲ㣬����ʽ������
    private Transform StoryUIRoot;     // ���±�����
    private Transform TipUIRoot;       // λ��UI���㣬������Ҫ��ʾ��Ϣ��
    private Transform MenuPopUIRoot;
    private Transform MessageUIRoot;
    private Transform DeathUIRoot;

    //����� �����ͷֺ�һ���ܿ�
    private Dictionary<string, GameObject> m_dicTipUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicBaseUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicPopUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicStoryUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicMenuPopUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicMessageUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicDeathUI = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> m_dicCacheUI = new Dictionary<string, GameObject>();

    //���ü���
    private Dictionary<string, int> m_dicWaitLoad = new Dictionary<string, int>();

    //����
    private static UIManager m_instance;
    public static UIManager Instance()
    {
        return m_instance;
    }

    private void Awake()
    {
        m_dicTipUI.Clear();
        m_dicBaseUI.Clear();
        m_dicPopUI.Clear();
        m_dicStoryUI.Clear();
        m_dicMenuPopUI.Clear();
        m_dicMessageUI.Clear();
        m_dicDeathUI.Clear();
        m_dicCacheUI.Clear();
        m_instance = this;

        BaseUIRoot = gameObject.transform.Find("BaseUIRoot");

        PopUIRoot = gameObject.transform.Find("PopUIRoot");

        StoryUIRoot = gameObject.transform.Find("StoryUIRoot");

        TipUIRoot = gameObject.transform.Find("TipUIRoot");

        MenuPopUIRoot = gameObject.transform.Find("MenuPopUIRoot");

        MessageUIRoot = gameObject.transform.Find("MessageUIRoot");

        DeathUIRoot = gameObject.transform.Find("DeathUIRoot");

        BaseUIRoot.gameObject.SetActive(true);
        TipUIRoot.gameObject.SetActive(true);
        PopUIRoot.gameObject.SetActive(true);
        StoryUIRoot.gameObject.SetActive(true);
        MenuPopUIRoot.gameObject.SetActive(true);
        MessageUIRoot.gameObject.SetActive(true);
        DeathUIRoot.gameObject.SetActive(true);
    }
    
    //
    public static bool LoadItem(UIPathData pathData, OnLoadUIItemDelegate delLoadItem, object param = null)
    {
        //if (null == m_instance)
        //{
        //    LogModule.ErrorLog("game manager is not init");
        //    return false;
        //}

        m_instance.LoadUIItem(pathData, delLoadItem, param);
        return true;
    }

    // չʾUI���������Ͳ�ͬ��������ͬ��Ϊ
    public static bool ShowUI(UIPathData pathData, OnOpenUIDelegate delOpenUI = null, object param = null)
    {
        if (null == m_instance)
        {
            Debug.LogError("game manager is not init");
            return false;
        }

        m_instance.AddLoadDicRefCount(pathData.name);

        Dictionary<string, GameObject> curDic = null;
        switch (pathData.uiType)
        {
            case UIPathData.UIType.TYPE_BASE:
                curDic = m_instance.m_dicBaseUI;
                break;
            case UIPathData.UIType.TYPE_POP:
                curDic = m_instance.m_dicPopUI;
                break;
            case UIPathData.UIType.TYPE_STORY:
                curDic = m_instance.m_dicStoryUI;
                break;
            case UIPathData.UIType.TYPE_TIP:
                curDic = m_instance.m_dicTipUI;
                break;
            case UIPathData.UIType.TYPE_MENUPOP:
                curDic = m_instance.m_dicMenuPopUI;
                break;
            case UIPathData.UIType.TYPE_MESSAGE:
                curDic = m_instance.m_dicMessageUI;
                break;
            case UIPathData.UIType.TYPE_DEATH:
                curDic = m_instance.m_dicDeathUI;

                break;
            default:
                return false;
        }

        if (null == curDic)
        {
            return false;
        }

        if (m_instance.m_dicCacheUI.ContainsKey(pathData.name))
        {
            if (!curDic.ContainsKey(pathData.name))
            {
                curDic.Add(pathData.name, m_instance.m_dicCacheUI[pathData.name]);
            }

            m_instance.m_dicCacheUI.Remove(pathData.name);
        }

        if (curDic.ContainsKey(pathData.name))
        {
            curDic[pathData.name].SetActive(true);
            m_instance.DoAddUI(pathData, curDic[pathData.name], delOpenUI, param);
            return true;
        }

        m_instance.LoadUI(pathData, delOpenUI, param);

        return true;
    }

    void AddLoadDicRefCount(string pathName)
    {
        if (m_dicWaitLoad.ContainsKey(pathName))
        {
            m_dicWaitLoad[pathName]++;
        }
        else
        {
            m_dicWaitLoad.Add(pathName, 1);
        }
    }


    public static void CloseUIByID(int tableID)
    {
        if (null == m_instance)
        {
            return;
        }
        //Tab_UIPath curTabPath = TableManager.GetUIPathByID(tableID, 0);
        //if (null == curTabPath)
        //{
        //    LogModule.ErrorLog("cur ui is not set in table" + tableID);
        //    return;
        //}

        //if (!UIPathData.m_DicUIInfo.ContainsKey(curTabPath.Path))
        //{
        //    LogModule.ErrorLog("cur ui is not set in table " + curTabPath.Path);
        //    return;
        //}

        //UIPathData curPathData = UIPathData.m_DicUIInfo[curTabPath.Path];
        //CloseUI(curPathData);
    }

    // ����չʾUI
    public static bool ShowUIByID(int tableID, OnOpenUIDelegate delOpenUI = null, object param = null)
    {
        return false;
        //if (null == m_instance)
        //{
        //    LogModule.ErrorLog("game manager is not init");
        //    return false;
        //}

        //Tab_UIPath curTabPath = TableManager.GetUIPathByID(tableID, 0);
        //if (null == curTabPath)
        //{
        //    LogModule.ErrorLog("cur ui is not set in table" + tableID);
        //    return false;
        //}

        //if (!UIPathData.m_DicUIInfo.ContainsKey(curTabPath.Path))
        //{
        //    LogModule.ErrorLog("cur ui is not set in table" + curTabPath.Path);
        //    return false;
        //}

        //UIPathData curData = UIPathData.m_DicUIInfo[curTabPath.Path];
        //return UIManager.ShowUI(curData, delOpenUI, param);

    }
    public static void CloseUI(UIPathData pathData)
    {
        if (null == m_instance)
        {
            return;
        }

        //int MaxCloseCount = PlayerPreferenceData.MaxCleanUICount;
        //if (MaxCloseCount > 6)
        //{
        //    MaxCloseCount = 6;
        //}

        //�ر�MaxCloseCount��UI��ʱ������GC
        //if (++m_sCloseUICount >= MaxCloseCount)
        //{
        //    Resources.UnloadUnusedAssets();
        //    GC.Collect();
        //    m_sCloseUICount = 0;
        //   // LogModule.DebugLog("CloseUI GC 1");
        //}
        //else
        //{
        //    //������ͣ������ͼ��PK����¥, ���ˣ��������棬��飬ÿ�δ򿪶�����
        //    if (pathData.name == "ActivityController" ||
        //        pathData.name == "SwordsManController" ||
        //        pathData.name == "SceneMapRoot" ||
        //        pathData.name == "PKSetRoot" ||
        //        pathData.name == "Restaurant" ||
        //        pathData.name == "BelleController" ||
        //        pathData.name == "BackPackRoot" ||
        //        pathData.name == "PartnerAndMountRoot")
        //    {
        //        Resources.UnloadUnusedAssets();
        //        GC.Collect();
        //        m_sCloseUICount = 0;
        //        //LogModule.DebugLog("CloseUI GC 2 " + pathData.name);
        //    }
        //}
        //LogModule.DebugLog("m_sCloseUICount : " + m_sCloseUICount + " MaxCloseCount= " + MaxCloseCount);

        //if (!m_GCTimerGo)
        //{
        //    //�ر�UI��ʱ�������Ҳ������������������˳������һ���ڴ�
        //    //����ر�UI��ʱ�������Ҫ��������ˣ����Ҫ�ų���
        //    //Ŀǰ������������֮�����������֮�����ͳһ����
        //    if (pathData.name != "NewPlayerGuidRoot")
        //    {
        //        m_GCTimerGo = true;
        //        m_GCWaitTime = Time.fixedTime;
        //    }
        //}

        //         if (pathData.name.Equals("BelleController"))
        //         {
        //             Resources.UnloadUnusedAssets();
        //           //  GC.Collect();
        //             //LogModule.DebugLog("BelleController GC " + pathData.name);
        //         }
        Resources.UnloadUnusedAssets();
        m_instance.RemoveLoadDicRefCount(pathData.name);
        switch (pathData.uiType)
        {
            case UIPathData.UIType.TYPE_BASE:
                m_instance.CloseBaseUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_POP:
                m_instance.ClosePopUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_STORY:
                m_instance.CloseStoryUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_TIP:
                m_instance.CloseTipUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_MENUPOP:
                m_instance.CloseMenuPopUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_MESSAGE:
                m_instance.CloseMessageUI(pathData.name);
                break;
            case UIPathData.UIType.TYPE_DEATH:
                m_instance.CloseDeathUI(pathData.name);
                break;
            default:
                break;
        }

        if (pathData.uiGroupName != null && pathData.isMainAsset)
        {
            //AB���ͷ�
            //BundleManager.ReleaseLoginBundle();
        }
    }
    void DoLoadUIItem(UIPathData uiData, GameObject curItem, object fun, object param)
    {
        if (null != fun)
        {
            OnLoadUIItemDelegate delLoadItem = fun as OnLoadUIItemDelegate;
            delLoadItem(curItem, param);
        }
    }
    void DoAddUI(UIPathData uiData, GameObject curWindow, object fun, object param)
    {
        //        if (!m_dicWaitLoad.Remove(uiData.name))
        //        {
        //            return;
        //        }      
        if (null != curWindow)
        {
            Transform parentRoot = null;
            Dictionary<string, GameObject> relativeDic = null;
            switch (uiData.uiType)
            {
                case UIPathData.UIType.TYPE_BASE:
                    parentRoot = BaseUIRoot;
                    relativeDic = m_dicBaseUI;
                    break;
                case UIPathData.UIType.TYPE_POP:
                    parentRoot = PopUIRoot;
                    relativeDic = m_dicPopUI;
                    break;
                case UIPathData.UIType.TYPE_STORY:
                    parentRoot = StoryUIRoot;
                    relativeDic = m_dicStoryUI;
                    break;
                case UIPathData.UIType.TYPE_TIP:
                    parentRoot = TipUIRoot;
                    relativeDic = m_dicTipUI;
                    break;
                case UIPathData.UIType.TYPE_MENUPOP:
                    parentRoot = MenuPopUIRoot;
                    relativeDic = m_dicMenuPopUI;
                    break;
                case UIPathData.UIType.TYPE_MESSAGE:
                    parentRoot = MessageUIRoot;
                    relativeDic = m_dicMessageUI;
                    break;
                case UIPathData.UIType.TYPE_DEATH:
                    parentRoot = DeathUIRoot;
                    relativeDic = m_dicDeathUI;
                    break;
                default:
                    break;

            }

            if (uiData.uiType == UIPathData.UIType.TYPE_POP || uiData.uiType == UIPathData.UIType.TYPE_MENUPOP)
            {
                OnLoadNewPopUI(m_dicPopUI, uiData.name);
                OnLoadNewPopUI(m_dicMenuPopUI, uiData.name);
            }
            if (null != relativeDic && relativeDic.ContainsKey(uiData.name))
            {
                relativeDic[uiData.name].SetActive(true);
            }

            else if (null != parentRoot && null != relativeDic)
            {
                GameObject newWindow = GameObject.Instantiate(curWindow) as GameObject;

                if (null != newWindow)
                {
                    Vector3 oldScale = newWindow.transform.localScale;
                    newWindow.transform.parent = parentRoot;
                    newWindow.transform.localPosition = Vector3.zero;
                    newWindow.transform.localScale = oldScale;
                    relativeDic.Add(uiData.name, newWindow);
                    if (uiData.uiType == UIPathData.UIType.TYPE_MENUPOP)
                    {
                        LoadMenuSubUIShield(newWindow);
                    }
                }
            }

            if (uiData.uiType == UIPathData.UIType.TYPE_STORY)
            {
                BaseUIRoot.gameObject.SetActive(false);
                TipUIRoot.gameObject.SetActive(false);
                PopUIRoot.gameObject.SetActive(false);
                MenuPopUIRoot.gameObject.SetActive(false);
                MessageUIRoot.gameObject.SetActive(false);
                StoryUIRoot.gameObject.SetActive(true);
            }
            else if (uiData.uiType == UIPathData.UIType.TYPE_MENUPOP)
            {
                //if (null != PlayerFrameLogic.Instance() && null != MenuBarLogic.Instance())
                //{
                //    MenuBarLogic.Instance().gameObject.SetActive(false);
                //    PlayerFrameLogic.Instance().gameObject.SetActive(false);

                //    StartCoroutine(GCAfterOneSceond());
                //}
            }
            else if (uiData.uiType == UIPathData.UIType.TYPE_DEATH)
            {
                ReliveCloseOtherSubUI();
            }
            else if (uiData.uiType == UIPathData.UIType.TYPE_POP)
            {
                //LoadPopUIShield(newWindow);
                //if (PlayerFrameLogic.Instance() != null)
                //{
                //    PlayerFrameLogic.Instance().SwitchAllWhenPopUIShow(false);
                //    PlayerFrameLogic.Instance().gameObject.SetActive(false);
                //}
                //if (MenuBarLogic.Instance() != null)
                //{
                //    MenuBarLogic.Instance().gameObject.SetActive(false);
                //}
                //if (ItemTooltipsLogic.Instance() != null)
                //{
                //    ItemTooltipsLogic.Instance().gameObject.SetActive(false);
                //}
                //if (EquipTooltipsLogic.Instance() != null)
                //{
                //    EquipTooltipsLogic.Instance().gameObject.SetActive(false);
                //}

                //if (!(uiData.name.Equals("ServerChooseController") ||
                //    uiData.name.Equals("RoleCreate")))
                //{
                //    // StartCoroutine(GCAfterOneSceond());
                //}
            }
        }

        if (null != fun)
        {
            OnOpenUIDelegate delOpenUI = fun as OnOpenUIDelegate;
            delOpenUI(curWindow != null, param);
        }
    }
    public void HideAllUILayer()
    {
        BaseUIRoot.gameObject.SetActive(false);
        TipUIRoot.gameObject.SetActive(false);
        PopUIRoot.gameObject.SetActive(false);
        MenuPopUIRoot.gameObject.SetActive(false);
        MessageUIRoot.gameObject.SetActive(false);
        StoryUIRoot.gameObject.SetActive(false);
    }
    public void ShowAllUILayer()
    {
        BaseUIRoot.gameObject.SetActive(true);
        TipUIRoot.gameObject.SetActive(true);
        PopUIRoot.gameObject.SetActive(true);
        MenuPopUIRoot.gameObject.SetActive(true);
        MessageUIRoot.gameObject.SetActive(true);
        StoryUIRoot.gameObject.SetActive(true);
    }
    IEnumerator GCAfterOneSceond()
    {
        yield return new WaitForSeconds(1);

        // Resources.UnloadUnusedAssets();
        //System.GC.Collect();
    }
    void LoadUI(UIPathData uiData, OnOpenUIDelegate delOpenUI = null, object param1 = null)
    {
        GameObject curWindow = Resources.Load("Prefab/UI/" + uiData.name) as GameObject;
        if (null != curWindow)
        {
            DoAddUI(uiData, curWindow, delOpenUI, param1);
            //LogModule.ErrorLog("can not open controller path not found:" + path);
            return;
        }

        if (uiData.uiGroupName != null)
        {
            //TODO AB������
            GameObject objCacheBundle = null;// BundleManager.GetGroupUIItem(uiData);
            if (null != objCacheBundle)
            {
                DoAddUI(uiData, objCacheBundle, delOpenUI, param1);
                return;
            }
        }
        //StartCoroutine(BundleManager.LoadUI(uiData, DoAddUI, delOpenUI, param1));
    }
    //����Item
    void LoadUIItem(UIPathData uiData, OnLoadUIItemDelegate delLoadItem, object param = null)
    {
        //ͨ��Resources����
        GameObject curWindow = Resources.Load("Prefab/UI/" + uiData.name) as GameObject;
        if (null != curWindow)
        {
            DoLoadUIItem(uiData, curWindow, delLoadItem, param);
            return;
        }

        //ͨ��AB����ʽ����
        if (uiData.uiGroupName != null)
        {
            GameObject objCacheBundle = null;//BundleManager.GetGroupUIItem(uiData);
            if (null != objCacheBundle)
            {
                DoLoadUIItem(uiData, objCacheBundle, delLoadItem, param);
                return;
            }
        }

        //StartCoroutine(BundleManager.LoadUI(uiData, DoLoadUIItem, delLoadItem, param));
    }

    //�����µĴ��ڶ����UI
    private void OnLoadNewPopUI(Dictionary<string, GameObject> curList, string curName)
    {
        if (curList == null)
        {
            return;
        }

        List<string> objToRemove = new List<string>();

        if (curList.Count > 0)
        {
            objToRemove.Clear();
            foreach (KeyValuePair<string, GameObject> objs in curList)
            {
                if (curName == objs.Key)
                {
                    continue;
                }
                objs.Value.SetActive(false);
                objToRemove.Add(objs.Key);
                if (UIPathData.m_DicUIName.ContainsKey(objs.Key) && UIPathData.m_DicUIName[objs.Key].isDestroyOnUnload)
                {
                    DestroyUI(objs.Key, objs.Value);
                }
                else
                {
                    m_dicCacheUI.Add(objs.Key, objs.Value);
                }
            }

            for (int i = 0; i < objToRemove.Count; i++)
            {
                if (curList.ContainsKey(objToRemove[i]))
                {
                    curList.Remove(objToRemove[i]);
                }
            }
        }
    }

    static void LoadMenuSubUIShield(GameObject newWindow)
    {
        GameObject MenuSubUIShield = Instantiate(Resources.Load("Prefab/UI/MenuSubUIShield") as GameObject);
        if (MenuSubUIShield == null)
        {
            Debug.LogError("can not open MenuSubUIShield path not found");
           // LogModule.ErrorLog("can not open MenuSubUIShield path not found");
            return;
        }
        MenuSubUIShield.transform.parent = newWindow.transform;
        MenuSubUIShield.transform.localPosition = Vector3.zero;
        MenuSubUIShield.transform.localScale = Vector3.one;
    }

    void DestroyUI(string name, GameObject obj)
    {
        Destroy(obj);
        //BundleManager.OnUIDestroy(name);
    }

    bool RemoveLoadDicRefCount(string pathName)
    {
        if (!m_dicWaitLoad.ContainsKey(pathName))
        {
            return false;
        }

        m_dicWaitLoad[pathName]--;
        if (m_dicWaitLoad[pathName] <= 0)
        {
            m_dicWaitLoad.Remove(pathName);
        }

        return true;
    }

    void ClosePopUI(string name)
    {
        OnClosePopUI(m_dicPopUI, name);
    }

    void CloseStoryUI(string name)
    {
        if (TryDestroyUI(m_dicStoryUI, name))
        {
            BaseUIRoot.gameObject.SetActive(true);
            TipUIRoot.gameObject.SetActive(true);
            PopUIRoot.gameObject.SetActive(true);
            MenuPopUIRoot.gameObject.SetActive(true);
            MessageUIRoot.gameObject.SetActive(true);
            StoryUIRoot.gameObject.SetActive(true);
        }
    }

    void CloseBaseUI(string name)
    {
        if (m_dicBaseUI.ContainsKey(name))
        {
            m_dicBaseUI[name].SetActive(false);
        }
    }

    void CloseTipUI(string name)
    {
        TryDestroyUI(m_dicTipUI, name);
    }

    void CloseMenuPopUI(string name)
    {
        OnClosePopUI(m_dicMenuPopUI, name);
    }

    void CloseMessageUI(string name)
    {
        TryDestroyUI(m_dicMessageUI, name);
    }

    void CloseDeathUI(string name)
    {
        if (TryDestroyUI(m_dicDeathUI, name))
        {
            // �رո������ʱ �ָ��ڵ����ʾ
            m_instance.PopUIRoot.gameObject.SetActive(true);
            m_instance.MenuPopUIRoot.gameObject.SetActive(true);
            m_instance.TipUIRoot.gameObject.SetActive(true);
            m_instance.BaseUIRoot.gameObject.SetActive(true);
        }
    }


    private bool TryDestroyUI(Dictionary<string, GameObject> curList, string curName)
    {
        if (curList == null)
        {
            return false;
        }

        if (!curList.ContainsKey(curName))
        {
            return false;
        }

        //#if UNITY_ANDROID

        // < 768M UI�����л���
        if (SystemInfo.systemMemorySize < 768)
        {
            DestroyUI(curName, curList[curName]);
            curList.Remove(curName);

            Resources.UnloadUnusedAssets();
            GC.Collect();
            return true;
        }

        //#endif

        if (UIPathData.m_DicUIName.ContainsKey(curName) && !UIPathData.m_DicUIName[curName].isDestroyOnUnload)
        {
            curList[curName].SetActive(false);
            m_dicCacheUI.Add(curName, curList[curName]);
        }
        else
        {
            DestroyUI(curName, curList[curName]);
        }

        curList.Remove(curName);

        return true;
    }

    private void OnClosePopUI(Dictionary<string, GameObject> curList, string curName)
    {
        if (TryDestroyUI(curList, curName))
        {
            // �رյ������򿪵Ķ���UIʱ �ջص�����
            //if (null != PlayerFrameLogic.Instance())
            //{
            //    PlayerFrameLogic.Instance().gameObject.SetActive(true);
            //    if (PlayerFrameLogic.Instance().Fold)
            //    {
            //        PlayerFrameLogic.Instance().SwitchAllWhenPopUIShow(true);
            //    }
            //}
            //if (MenuBarLogic.Instance() != null)
            //{
            //    if (MenuBarLogic.Instance().Fold)
            //    {
            //        MenuBarLogic.Instance().gameObject.SetActive(true);
            //    }
            //}
        }
    }

    static void LoadPopUIShield(GameObject newWindow)
    {
        //if (GameManager.gameManager.RunningScene == (int)GameDefine_Globe.SCENE_DEFINE.SCENE_LOGIN ||
        //    GameManager.gameManager.RunningScene == (int)GameDefine_Globe.SCENE_DEFINE.SCENE_LOADINGSCENE)
        //{
        //    return;
        //}

        //GameObject PopUIBlack = ResourceManager.InstantiateResource("Prefab/UI/PopUIBlack") as GameObject;
        //if (PopUIBlack == null)
        //{
        //    LogModule.ErrorLog("can not open PopUIBlack path not found");
        //    return;
        //}
        //PopUIBlack.transform.parent = newWindow.transform;
        //PopUIBlack.transform.localPosition = Vector3.zero;
        //PopUIBlack.transform.localScale = Vector3.one;
    }


    static void ReliveCloseOtherSubUI()
    {
        // �ر�����PopUI
        List<string> uiKeyList = new List<string>();
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicPopUI)
        {
            uiKeyList.Add(pair.Key);
        }
        for (int i = 0; i < uiKeyList.Count; i++)
        {
            m_instance.ClosePopUI(uiKeyList[i]);
        }
        uiKeyList.Clear();
        // �ر�����MenuPopUI
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicMenuPopUI)
        {
            uiKeyList.Add(pair.Key);
        }
        for (int i = 0; i < uiKeyList.Count; i++)
        {
            m_instance.CloseMenuPopUI(uiKeyList[i]);
        }
        uiKeyList.Clear();
        // �ر�����TipUI
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicTipUI)
        {
            uiKeyList.Add(pair.Key);
        }
        for (int i = 0; i < uiKeyList.Count; i++)
        {
            m_instance.CloseTipUI(uiKeyList[i]);
        }
        uiKeyList.Clear();
        // �ر����г�CentreNotice�����MessageUI MessageUIRoot�ڵ㱣��������
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicMessageUI)
        {
            if (!pair.Key.Contains("CentreNotice"))
            {
                uiKeyList.Add(pair.Key);
            }
        }
        for (int i = 0; i < uiKeyList.Count; i++)
        {
            m_instance.CloseMessageUI(uiKeyList[i]);
        }
        uiKeyList.Clear();

        // �жϾ���Ի�
        //if (StoryDialogLogic.Instance() != null)
        //{
        //    CloseUI(UIInfo.StoryDialogRoot);
        //}
        // �ж�ʫ�ʶԻ�
        //if (ShiCiLogic.Instance() != null)
        //{
        //    CloseUI(UIInfo.ShiCiRoot);
        //}
        // �жϽ��׶Ի�
        //if (JianPuLogic.Instance() != null)
        //{
        //    CloseUI(UIInfo.JianPuRoot);
        //}

        // ���ض���UI�ڵ�
        m_instance.PopUIRoot.gameObject.SetActive(false);
        m_instance.MenuPopUIRoot.gameObject.SetActive(false);
        m_instance.TipUIRoot.gameObject.SetActive(false);
        m_instance.BaseUIRoot.gameObject.SetActive(false);
    }
    static public void NewPlayerGuideCloseSubUI()
    {
        // �ر�����PopUI
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicPopUI)
        {
            m_instance.ClosePopUI(pair.Key);
            break;
        }
        // �ر�����MenuPopUI
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicMenuPopUI)
        {
            m_instance.CloseMenuPopUI(pair.Key);
            break;
        }
        // �ر�����TipUI
        foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicTipUI)
        {
            m_instance.CloseTipUI(pair.Key);
            break;
        }
        // �ر�����MessageUI
        //         foreach (KeyValuePair<string, GameObject> pair in m_instance.m_dicMessageUI)
        //         {
        //             m_instance.CloseMessageUI(pair.Key);
        //             break;
        //         }
    }

    public static bool IsSubUIShow()
    {
        if (m_instance != null)
        {
            return m_instance.SubUIShow();
        }
        return false;
    }
    bool SubUIShow()
    {
        /**
		 * ��ֹ�����������ֱ�ӷ���false
		 * ����Ѱ·��NPC�Ĺ����У����С�������װ�����ӣ�����װ��Tips������Ѱ·�����󣬵���NPC�Ի���壬�����Ի��󣬷���ٿذ�ťʧЧ
		 */
        //if (m_dicPopUI.Count + m_dicStoryUI.Count + m_dicTipUI.Count + m_dicMenuPopUI.Count > 0)
        //{
        //    return true;
        //}
        //else
        //{
        return false;
        //}
    }

    GameObject AddObjToRoot(string name)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.name = name;
        return obj;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        m_instance = null;
    }
}
