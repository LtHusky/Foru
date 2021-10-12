using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class Pump : MonoBehaviour
{
	// Skimmer.
	[Header("Skimmer")]
	public GameObject floatPiece;
	public BoxCollider floatCol;
	Vector3 floatColOrigin;
	float floatPieceAdjustment = 0.05f;

	int inlateGap;

	// Pump.
	[Header("Pump")]
	int pumpSpeed = 0;

	public float pumpPercentage;

	public float StartOilInWater = 1000;
	public float PumpedUpOil;
	float runningPumped = 0;
	bool pumpOn;

	// Oil & water.
	[Header("Oil & water")]
	public GameObject oilHolder;
	public GameObject oilLayer;

	float[] oilHeights = { 0.2f, 0.4f, 0.6f, 0.8f, 1f };
	float oilHeight = 1;
	float oilHeightSteps = 100;
	float oilHeightPercentage = 100;

	public MeshRenderer waterMatTM;
	public Shader waterShader;

	public Water2DTool.Water2D_Tool tool;

	public Color endColor_Shallow;
	public Color endColor_Deep;

	// Points system.
	[Header("Points")]
	float startOilHeight;
	float currentTime;
	float waterPumped;
	float points;
	string endTime;

	// UI.
	[Header("UI")]
	public TextMeshProUGUI oilLayerThicknessText;
	public TextMeshProUGUI inlateGapText;
	public TextMeshProUGUI percentageText;
	public TextMeshProUGUI pumpSpeedText;
	[Space]
	public GameObject warningSign;
	public TextMeshProUGUI warningText;
	[Space]
	public TextMeshProUGUI endTimerText;
	public TextMeshProUGUI endOilText;
	public TextMeshProUGUI endWaterText;
	public TextMeshProUGUI endPointsText;

	// Animation.
	[Header("Animation")]
	public Animator animator;

	// Setup.
	void Start()
	{
		// Random oil height.
		oilHeight = oilHeights[UnityEngine.Random.Range(0, 4)];
		oilHolder.transform.localScale = new Vector3(oilLayer.transform.localScale.x, oilHeight, oilLayer.transform.localScale.z);

		startOilHeight = oilHeight;

		floatColOrigin = floatCol.center;
	}

	void FixedUpdate()
	{
		TextUpdate();

		// Activate oil pump.
		if (pumpOn && inlateGap >= 1)
		{
			float tempPumpPercentage = PumpedUpOil / StartOilInWater;
			pumpPercentage = tempPumpPercentage * 100;

			UpdateTimer();
			Oilpump();
			WaterSettings();
			InlateGapSettings();
			PumpSpeedSettings();
		}
	}

	// UI Buttons.
	public void PumpOn()
	{
		if (pumpOn)
        {
			pumpSpeed = 0;
			pumpOn = false;
		}
        else
        {
			pumpSpeed = 1;
			pumpOn = true;
		}
	}
	public void PumpSpeedUp()
	{
		if (pumpSpeed <= 9 && pumpSpeed >= 0 && pumpOn == true)
			pumpSpeed++;
	}
	public void PumpSpeedDown()
	{
		if (pumpSpeed >= 2)
			pumpSpeed--;
	}
	public void PumpOff()
	{
		pumpSpeed = 0;
		pumpOn = false;
	}
	
	public void OpenGap()
    {
		if (inlateGap < 5 && inlateGap >= 0)
        {
			floatPiece.transform.position += new Vector3(0, floatPieceAdjustment, 0);
			floatCol.center += new Vector3(0, 0, floatPieceAdjustment);
			inlateGap++;
		}
	}
	public void CloseGap()
    {
		if (inlateGap <= 5 && inlateGap > 0)
		{
			floatPiece.transform.position -= new Vector3(0, floatPieceAdjustment, 0);
			floatCol.center -= new Vector3(0, 0, floatPieceAdjustment);
			inlateGap--;
		}
	}

	IEnumerator EnableWarning(string content, bool pumpWater)
    {
		bool looped = false;

		if (!looped)
		{
			warningSign.SetActive(true);
			warningText.text = content;
			yield return new WaitForSeconds(3);
			warningSign.SetActive(false);
			
			looped = true;
		}

		if (pumpWater)
		{
			waterPumped += Time.deltaTime;
		}
	}
    public void TextUpdate()
	{
		float tempThickness = oilHeight * 10;
		oilLayerThicknessText.text = "Oil thickness: " + tempThickness.ToString("0.0") + "CM";
		inlateGapText.text = "Inlate gap: " + inlateGap + "CM";
		percentageText.text = (int)pumpPercentage + "%";
		pumpSpeedText.text = "Speed: " + pumpSpeed + " m3/h";
	}

	// Timer.
	void UpdateTimer()
    {
		currentTime += Time.deltaTime;
		TimeSpan timePlaying = TimeSpan.FromSeconds(currentTime);
		endTime = "Time spent: " + timePlaying.ToString("mm':'ss'.'ff");
	}

	// Game settings.
	void WaterSettings()
	{
		// Calculate & set oil height.
		oilHeightPercentage = 100 - pumpPercentage;

		if (oilHeightPercentage <= oilHeightSteps - 1)
		{
			oilHeight -= startOilHeight / 100;
			oilHeightSteps -= 1;

			// Scale update.
			Vector3 oilLayerScale = new Vector3(oilLayer.transform.localScale.x, oilHeight, oilLayer.transform.localScale.z);
			oilHolder.transform.localScale = oilLayerScale;
		}
	}

	void InlateGapSettings()
	{
		float temp = oilHeight * 10;

		// Check inlate gap settings.
		if (inlateGap > 1 && inlateGap > temp)
        {
			//slow down pump
			EnableWarning("Skimmer is pumping water!", true);
		}
		else if (inlateGap <= 2 && pumpSpeed >= 4)
        {
			//slow down pump
			floatCol.center = new Vector3(0, 0, -0.5f);
			PumpOff();

			StartCoroutine(EnableWarning("Skimmer is rising out of the water!", false));
		}
        else
        {
			floatCol.center = Vector3.Lerp(floatCol.center, floatColOrigin, 1 * Time.deltaTime); // fix!
		}
    }

	void PumpSpeedSettings()
	{
		if (pumpPercentage <= 10)
        {
			if (pumpSpeed > 1)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
        else if (pumpPercentage > 10 && pumpPercentage <= 15)
        {
			if (pumpSpeed > 2)
            {
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
            }
        }
		else if (pumpPercentage > 15 && pumpPercentage <= 20)
		{
			if (pumpSpeed > 4)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
		else if (pumpPercentage > 20 && pumpPercentage <= 25)
		{
			if (pumpSpeed > 6)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}

		else if (pumpPercentage > 75 && pumpPercentage <= 85)
		{
			if (pumpSpeed > 6)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
		else if (pumpPercentage > 85 && pumpPercentage <= 90)
		{
			if (pumpSpeed > 4)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
		else if (pumpPercentage > 90 && pumpPercentage <= 95)
		{
			if (pumpSpeed > 2)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
		else if (pumpPercentage > 95 && pumpPercentage <= 100)
		{
			if (pumpSpeed > 1)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!", true));
			}
		}
	}

	// Pump.
	void Oilpump()
	{
		// Oilpump process.
		if (PumpedUpOil < StartOilInWater)
		{
			if (pumpSpeed > 0)
			{
				runningPumped += Time.deltaTime;
				if (runningPumped < 1.0f)
				{
					runningPumped *= pumpSpeed;
				}
				else
				{
					runningPumped = 0;
				}
				PumpedUpOil += runningPumped;
			}
		}
		else
		{
			EndGame();
		}
	}

	// End game.
	void EndGame()
    {
		PumpedUpOil = StartOilInWater / 10;
		oilHeight = 0.001f;
		pumpPercentage = 100;

		waterMatTM.material.shader = waterShader;
		waterMatTM.material.SetColor("_DeepWaterColor", endColor_Deep);
		waterMatTM.material.SetColor("_ShallowWaterColor", endColor_Shallow);

		oilLayer.SetActive(false);
		PumpOff();

		animator.SetTrigger("FadeIn");


		endTimerText.text = endTime;
		endOilText.text = "Oil pumped: " + PumpedUpOil.ToString("000") + "L";
		endWaterText.text = "Water pumped: " + waterPumped.ToString("000") + "L";
		
		points = PumpedUpOil -= waterPumped;
		endPointsText.text = "Points: " + points.ToString("000");
	}
}
/* fixes needed:
 * 212
 * 217
 * 255
 * fix float scale when oil deletion
 * fading animation main menu
 * fix second camera resolution
 */