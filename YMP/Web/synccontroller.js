var change_time_state = true;

let video;
let audio;
let videoTrack;
let audioTrack;

const playing = 1;
const pause = 2;
const ended = 0;
const cued = -1;

let onStateChange = function () { };
let onCurrentTimeChange = function () { };
let onDurationChange = function () { };

function loadElement() {
    if (!video) {
        video = document.getElementById("myVideo");
        videoTrack = document.getElementById("myVideoTrack");

        video.addEventListener('play', onPlay);
        video.addEventListener('pause', onPause);
        video.addEventListener('ended', onEnded);
        video.addEventListener('timeupdate', onTimeupdate);
        video.addEventListener('durationchange', onDurationupdate);
        video.addEventListener('error', onVideoError);
    }

    if (!audio) {
        audio = document.getElementById("myAudio");
        audioTrack = document.getElementById("myAudioTrack");

        audio.addEventListener('error', onAudioError);
    }
}

function onPlay() {
    if (change_time_state) {
        audio.currentTime = video.currentTime;
        change_time_state = false;
    }
    audio.play();

    onStateChange(playing);
}

function onPause() {
    audio.pause();
    change_time_state = true;

    onStateChange(pause);
}

function onEnded() {
    onStateChange(ended);
}

var previousTime;
function onTimeupdate() {
    if (Math.abs(video.currentTime - audio.currentTime) > 0.2) {
        audio.currentTime = video.currentTime;
        audio.play();
    }

    var currentTime = parseInt(video.currentTime);
    if (currentTime && previousTime != currentTime) {
        previousTime = currentTime;
        onCurrentTimeChange(currentTime);
    }
}

var previousDuration;
function onDurationupdate() {
    var currentDuration = parseInt(video.duration);
    if (currentDuration && previousDuration != currentDuration) {
        previousDuration = currentDuration;
        onDurationChange(currentDuration);
    }
}

function onVideoError() {
    console.log(video.error);

    if (currVideoStreams.length > 0 && currVideoStreams.length > currVideoIndex) {
        currVideoIndex++;
        loadVideo(currVideoStreams[currVideoIndex]);
        seekTo(previousTime);
    }
}

function onAudioError() {
    console.log(audio.error);

    if (currAudioStreams.length > 0 && currAudioStreams.length > currAudioIndex) {
        currAudioIndex++;
        loadAudio(currAudioStreams[currAudioIndex]);
        seekTo(previousTime);
    }
}

var currVideoIndex = 0;
var currAudioIndex = 0;
var currVideoStreams = [];
var currAudioStreams = [];

async function loadVideoById(id) {
    stop();
    var [videoStreams, audioStreams] = await getVideoInfo(id);

    currVideoStreams = videoStreams;
    currAudioStreams = audioStreams;

    loadVideo(currVideoStreams[0]);
    loadAudio(currAudioStreams[0]);
}

function loadVideo(v) {
    console.log(v);
    video.src = v.url;
    video.type = v.mime;
}

function loadAudio(a) {
    console.log(a);
    audio.src = a.url;
    audio.type = a.mime;
}

function playVideo() {
    video.play();
}

function pauseVideo() {
    video.pause();
}

function stop() {
    video.src = "";
    audio.src = "";
    currVideoStreams = [];
    currAudioStreams = [];
    currAudioIndex = 0;
    currVideoIndex = 0;
    previousDuration = 0;
    previousTime = 0;
}

function seekTo(position) {
    video.currentTime = position;
    audio.currentTime = position;
}

function mute() {
    audio.muted = true;
}

function unMute() {
    audio.muted = false;
}
