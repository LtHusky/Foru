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

	float inlateGap;
	public float wheelValue;

	// Pump.
	[Header("Pump")]
	public float pumpSpeed = 0;

	public float pumpPercentage;

	float[] oilAmounts = { 2000, 2500, 3000, 3500, 4000 };
	public float startOilInWater = 1000;
	public float PumpedUpOil;
	float runningPumped = 0;
	bool pumpOn;
	bool pumpingWater;

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
	float bestTime = 30;
	float endTime;
	string endTimeString;

	float waterPumped;
	float bestWaterScore = 1;
	float endWaterScore;

	float points;

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
	bool warningAbleToCall = true;

	// Animation.
	[Header("Animation")]
	public GameObject fadeImage;
	public Animator animator;

	// Setup.
	void Start()
	{
		// Random oil amount.
		int randomIDX = UnityEngine.Random.Range(0, 4);
		oilHeight = oilHeights[randomIDX];
		startOilInWater = oilAmounts[randomIDX];

		oilHolder.transform.localScale = new Vector3(oilLayer.transform.localScale.x, oilHeight, oilLayer.transform.localScale.z);

		startOilHeight = oilHeight;

		floatColOrigin = floatCol.center;
	}

	void FixedUpdate()
	{
		TextUpdate();
		InlateGapSettings();

		// Activate oil pump.
		if (pumpOn && inlateGap > 0)
		{
			float tempPumpPercentage = PumpedUpOil / startOilInWater;
			pumpPercentage = tempPumpPercentage * 100;

			UpdateTimer();
			Oilpump();
			WaterSettings();
			PumpSpeedSettings();
			PumpWater();
		}
	}

	// UI Buttons.
	public void SetSliderSpeed(float value)
	{
        if (!pumpOn)
        {
			pumpOn = true;
        }
		pumpSpeed = value;
	}
	public void PumpOff()
	{
		pumpSpeed = 0;
		pumpOn = false;
		pumpingWater = false;
	}

	IEnumerator EnableWarning(string content)
    {
		if (!warningSign.activeSelf)
        {
			warningText.text = content;
			warningSign.SetActive(true);

			yield return new WaitForSeconds(3);
			warningSign.SetActive(false);
			warningAbleToCall = true;
		}
	}

    public void TextUpdate()
	{
		float tempThickness = oilHeight * 10;
		oilLayerThicknessText.text = "Oil thickness: " + tempThickness.ToString("0.0") + "CM";
		inlateGapText.text = "Inlate gap: " + inlateGap.ToString("0.0") + "CM";
		percentageText.text = (int)pumpPercentage + "%";
		pumpSpeedText.text = "Speed: " + pumpSpeed + " m3/h";
	}

	// Timer.
	void UpdateTimer()
    {
		currentTime += Time.deltaTime;
		TimeSpan timePlaying = TimeSpan.FromSeconds(currentTime);
		endTimeString = "Time spent: " + timePlaying.ToString("mm':'ss'.'ff");
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
		float tempHeight = oilHeight * 10;

		inlateGap = wheelValue;
		inlateGap *= 2.5f;

		float adjustment = inlateGap / 20;
		floatCol.center = new Vector3(0, 0, 0.13f + adjustment);
		floatPiece.transform.position = new Vector3(0, gameObject.transform.position.y + adjustment - 0.25f, gameObject.transform.position.z);

		// Check inlate gap settings.
		if (inlateGap > 1 && inlateGap > tempHeight && pumpSpeed > 0)
        {
			//slow down pump!!!
			if (warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
		else if (inlateGap <= 2 && pumpSpeed >= 40 && inlateGap > 0)
        {
			//slow down pump!!!
			floatCol.center = new Vector3(0, 0, -0.5f);
			PumpOff();

			if (warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is rising out of the water!"));
				warningAbleToCall = false;
			}
		}
        else
        {
			floatCol.center = Vector3.Lerp(floatCol.center, floatColOrigin, 1 * Time.deltaTime);

			pumpingWater = false;
		}
    }

	void PumpSpeedSettings()
	{
		// OPTIMALIZE!!!
		if (pumpPercentage <= 10 && pumpSpeed > 20 && warningAbleToCall)
        {
			StartCoroutine(EnableWarning("Skimmer is pumping water!"));
			pumpingWater = true;
			warningAbleToCall = false;
		}
        else if (pumpPercentage > 10 && pumpPercentage <= 15)
        {
			if (pumpSpeed > 30 && warningAbleToCall)
            {
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
            }
        }
		else if (pumpPercentage > 15 && pumpPercentage <= 20)
		{
			if (pumpSpeed > 40 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
		else if (pumpPercentage > 20 && pumpPercentage <= 25)
		{
			if (pumpSpeed > 60 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}

		else if (pumpPercentage > 75 && pumpPercentage <= 85)
		{
			if (pumpSpeed > 60 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
		else if (pumpPercentage > 85 && pumpPercentage <= 90)
		{
			if (pumpSpeed > 40 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
		else if (pumpPercentage > 90 && pumpPercentage <= 95)
		{
			if (pumpSpeed > 30 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
		else if (pumpPercentage > 95 && pumpPercentage <= 100)
		{
			if (pumpSpeed > 20 && warningAbleToCall)
			{
				StartCoroutine(EnableWarning("Skimmer is pumping water!"));
				pumpingWater = true;
				warningAbleToCall = false;
			}
		}
	}

	// Pump.
	void Oilpump()
	{
		// Oilpump process.
		if (PumpedUpOil < startOilInWater)
		{
			if (pumpSpeed > 0)
			{
				runningPumped += Time.deltaTime;
				if (runningPumped < 1.0f)
				{
					runningPumped *= pumpSpeed / 10;
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

	void PumpWater()
    {
		if (pumpingWater)
        {
			float speedMultiplier = pumpSpeed * 2.777777777777778f;
			waterPumped += Time.deltaTime * speedMultiplier;
        }
    }

	// End game.
	void EndGame()
    {
		// End game.
		PumpedUpOil = startOilInWater / 10;
		oilHeight = 0.001f;
		pumpPercentage = 100;

		waterMatTM.material.shader = waterShader;
		waterMatTM.material.SetColor("_DeepWaterColor", endColor_Deep);
		waterMatTM.material.SetColor("_ShallowWaterColor", endColor_Shallow);

		oilLayer.SetActive(false);
		PumpOff();

		// To summary screen.
		fadeImage.SetActive(true);
		animator.SetTrigger("FadeIn");

		float tempWaterPumped = waterPumped / 100;

		endTime = 60 * Mathf.Pow(1.1f, bestTime - currentTime);
		endWaterScore = 40 * Mathf.Pow(1.05f, bestWaterScore - tempWaterPumped);

		points = endTime + endWaterScore;
		points = Mathf.Clamp(points, 0, 100);

		endTimerText.text = endTimeString;
		endOilText.text = "Oil pumped: " + PumpedUpOil.ToString("000") + "L";
		endWaterText.text = "Water pumped: " + waterPumped.ToString("000") + "L";
		endPointsText.text = "Points: " + points.ToString("000");
	}
}