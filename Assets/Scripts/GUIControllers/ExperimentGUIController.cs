﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentGUIController : DataGUIController {
    private const string Msg_Start = "Experiment Started";
    private const string Msg_Stop = "Experiment Stopped";

    //Drag from Unity Editor
    public InputField trialIntermissionFixedField;
    public InputField trialIntermissionMaxField;
    public InputField trialIntermissionMinField;

    public InputField saveLocationField;
    public InputField sessionIntermissionDurationField;
    public InputField timeoutDurationField;
    public InputField timeLimitField;

    public Toggle fixedTrailIntermissionToggle;
    public Toggle randomTrailIntermissionToggle;

    public Toggle posterEnableValid;
    public Toggle sessionIntermissionValid;
    public Toggle timeoutDurationValid;
    public Toggle timeLimitValid;

    public Toggle willPauseAtNextTrialToggle;
    public Toggle restartOnTrialFailToggle;
    public Toggle resetPositionOnTrialToggle;

    public Button browseSaveLocation;
    public Button startStopButton;
    public Button pauseButton;

    public ExperimentController experimentController;
    public FileBrowser fileBrowser;

    private void Awake() {
        trialIntermissionFixedField.onEndEdit.AddListener(OnTrialIntermissionFixedEndEdit);
        trialIntermissionMaxField.onEndEdit.AddListener(OnTrialIntermissionMaxEndEdit);
        trialIntermissionMinField.onEndEdit.AddListener(OnTrialIntermissionMinEndEdit);

        saveLocationField.onEndEdit.AddListener(OnSaveLocationFieldEndEdit);
        sessionIntermissionDurationField.onEndEdit.AddListener(OnSessionIntermissionFieldEndEdit);
        timeoutDurationField.onEndEdit.AddListener(OnTimeoutDurationFieldEndEdit);
        timeLimitField.onEndEdit.AddListener(OnTimeLimitFieldEndEdit);

        fixedTrailIntermissionToggle.onValueChanged.AddListener(OnFixedTrialIntermissionToggleChanged);
        randomTrailIntermissionToggle.onValueChanged.AddListener(OnRandomTrialIntermissionToggleChanged);

        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        startStopButton.onClick.AddListener(OnExperimentStartStopButtonClicked);
        browseSaveLocation.onClick.AddListener(OnBrowseSavelocationClicked);

        restartOnTrialFailToggle.onValueChanged.AddListener(toggleRestartOnTrialFail);
        resetPositionOnTrialToggle.onValueChanged.AddListener(toggleResetPosition);
    }

    private void toggleRestartOnTrialFail(bool isOn) {
        experimentController.restartOnTrialFail = isOn;
    }

    private void toggleResetPosition(bool isOn) {
        experimentController.resetPositionOnTrial = isOn;
    }

    private void OnPauseButtonClicked() {
        willPauseAtNextTrialToggle.isOn = experimentController.TogglePause();
    }

    private void OnExperimentStartStopButtonClicked() {
        if (!experimentController.started) {
            Console.Write("Experiment Started");
            experimentController.StartExperiment();
        }
        else {
            Console.Write("Experiment Stopped");
            experimentController.StopExperiment();
        }
    }

    private void OnBrowseSavelocationClicked() {
        fileBrowser.OnFileBrowserExit += OnFileBrowserExit;
        string initialDir = Application.dataPath;

        if (!string.IsNullOrEmpty(saveLocationField.text) && Directory.Exists(saveLocationField.text)) {
            initialDir = saveLocationField.text;
        }

        fileBrowser.Show(initialDir);
    }

    private void OnFileBrowserExit(string path) {
        fileBrowser.OnFileBrowserExit -= OnFileBrowserExit;

        if (!string.IsNullOrEmpty(path)) {
            saveLocationField.text = path;
            OnSaveLocationFieldEndEdit(path);
        }
    }

    private void OnFixedTrialIntermissionToggleChanged(bool value) {
        Session.isTrailIntermissionRandom = !value;
    }
    private void OnRandomTrialIntermissionToggleChanged(bool value) {
        Session.isTrailIntermissionRandom = value;
    }

    private void OnTimeLimitFieldEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            Session.trialTimeLimit = duration;
        }
        else {
            text = Session.trialTimeLimit.ToString();
            Console.WriteError("Invalid Value");
        }
    }

    private void OnSessionIntermissionFieldEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            experimentController.SessionIntermissionDuration = duration;
        }
        else {
            text = experimentController.SessionIntermissionDuration.ToString();
        }
    }

    private void OnTimeoutDurationFieldEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            Session.timeoutDuration = duration;
        }
        else {
            text = Session.timeoutDuration.ToString();
        }
    }

    private void OnTrialIntermissionFixedEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            Session.fixedTrialIntermissionDuration = duration;
        }
        else {
            text = Session.fixedTrialIntermissionDuration.ToString();
        }
    }

    private void OnTrialIntermissionMaxEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            Session.maxTrialIntermissionDuration = duration;
        }
        else {
            text = Session.maxTrialIntermissionDuration.ToString();
        }
    }

    private void OnTrialIntermissionMinEndEdit(string text) {
        if (IsValidDuration(text, out int duration)) {
            Session.minTrialIntermissionDuration = duration;
        }
        else {
            text = Session.minTrialIntermissionDuration.ToString();
        }
    }

    private void OnSaveLocationFieldEndEdit(string text) {
        if (IsValidSaveLocation(text)) {
            experimentController.SaveLocation = text;
            SetInputFieldValid(saveLocationField);
        }
        else {
            SetInputFieldInvalid(saveLocationField);
        }
    }

    private bool IsValidSaveLocation(string path) {
        return Directory.Exists(path);
    }

    private bool IsValidDuration(string text, out int duration) {
        int result = -1;
        if (int.TryParse(text, out result)) {
            duration = result;
            return IsValidDuration(duration);
        }
        duration = result;
        return false;
    }

    private bool IsValidDuration(int duration) {
        return duration > 0;
    }

    public override void UpdateSettingsGUI() {
        trialIntermissionFixedField.text = Session.fixedTrialIntermissionDuration.ToString();
        trialIntermissionMaxField.text = Session.maxTrialIntermissionDuration.ToString();
        trialIntermissionMinField.text = Session.minTrialIntermissionDuration.ToString();

        saveLocationField.text = experimentController.SaveLocation;
        sessionIntermissionDurationField.text = experimentController.SessionIntermissionDuration.ToString();
        timeoutDurationField.text = Session.timeoutDuration.ToString();
        timeLimitField.text = Session.trialTimeLimit.ToString();

        //to bypass ToggleGroup bug
        if (Session.isTrailIntermissionRandom) {
            randomTrailIntermissionToggle.isOn = true;
        }
        else {
            fixedTrailIntermissionToggle.isOn = true;
        }

        resetPositionOnTrialToggle.isOn = experimentController.resetPositionOnTrial;
        restartOnTrialFailToggle.isOn = experimentController.restartOnTrialFail;
        IsValidSaveLocation(experimentController.SaveLocation);
        sessionIntermissionValid.isOn = IsValidDuration(experimentController.SessionIntermissionDuration);
        timeoutDurationValid.isOn = IsValidDuration(Session.trialTimeLimit);
        timeLimitValid.isOn = IsValidDuration(Session.trialTimeLimit);
    }
}