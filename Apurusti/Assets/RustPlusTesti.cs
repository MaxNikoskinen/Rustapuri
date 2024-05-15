using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RustPlusApi;
using System.Net.WebSockets;

public class RustPlusTesti : MonoBehaviour
{
    private RustPlus rustPlusApi;

    public void Lataa()
    {
        rustPlusApi = new RustPlus();
        rustPlusApi.SendConnectionInfo("", 0, 0, 0, false);
    }
}
