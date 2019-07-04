using System.Collections;
using System.Collections.Generic;
using MessageLibrary;
using UnityEngine;
using UnityEngine.UI;

/**
 * @Author : #SMARTDEVELOPERS#
 * @Time: #CREATIONDATE#
 * @Desc:
 */

public class UnityCefCommTest : MonoBehaviour
{
    private string _inSharedCommFile = "outSharedCommFile";
    private string _outSharedCommFile = "inSharedCommFile";
    private SharedCommServer _inServer,_outServer;
    // Start is called before the first frame update
    
    public Text _outMessage;
    public Button _button;
    public Image _image;
    private bool isShowConnectOnce = false;
    void Start()
    {
        _inServer = new SharedCommServer(false);
        _outServer = new SharedCommServer(true);
        
        _button.onClick.AddListener(() =>
        {
	        if (_outServer!=null && _outServer.GetIsOpen())
	        {
		        _outServer.WriteMessage(new EventPacket()
		        {
			        eventId = 1,
			        eventValue = "UnityMessage"
		        });
	        }
        });
    }

    // Update is called once per frame
    void Update()
    {
	    if (!_inServer.GetIsOpen())
	    {
		    _inServer.Connect(_inSharedCommFile);
	    }
	    else
	    {
		    if (!isShowConnectOnce)
		    {
			    _outMessage.text = "连接成功!";
			    isShowConnectOnce = true;
			    _image.gameObject.SetActive(true);
		    }

		    EventPacket ep = _inServer.GetMessage();
		    if (ep != null)
		    {
			    _outMessage.text = ep.eventValue;
			    Debug.Log(ep.eventId+","+ ep.eventValue);
		    }

            
	    }

	    if (!_outServer.GetIsOpen())
	    {
		    _outServer.Connect(_outSharedCommFile);
	    }
    }
    
    private void OnDestroy()
    {
	    if(_outServer!=null)_outServer.Dispose();
	    if(_inServer!=null)_inServer.Dispose();
    }
}
