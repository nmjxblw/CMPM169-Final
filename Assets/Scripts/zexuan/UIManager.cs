using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject perlinOffsetXSlider;
    public GameObject perlinOffsetYSlider;
    public GameObject roomNumberSlider;
    public GameObject applyButton;
    public GameObject roomText;
    public GameObject xText;
    public GameObject yText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRoomNumberSliderChange()
    {
        roomText.GetComponent<TextMeshProUGUI>().text = "Room Number: " + roomNumberSlider.GetComponent<Slider>().value;

    }

    public void OnPerlinOffsetXSliderChange()
    {
        xText.GetComponent<TextMeshProUGUI>().text = "Perlin Offset X: " + perlinOffsetXSlider.GetComponent<Slider>().value;
    }

    public void OnPerlinOffsetYSliderChange()
    {
        yText.GetComponent<TextMeshProUGUI>().text = "Perlin Offset Y: " + perlinOffsetYSlider.GetComponent<Slider>().value;
    }
}
