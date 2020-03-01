
let audio;
let audioTrack;

const playing = 1;
const pause = 2;
const ended = 0;
const cued = -1;

let onStateChange = function () { };
let onCurrentTimeChange = function () { };
let onDurationChange = function () { };

function loadElement() {
  if (!audio) {
    audio = document.getElementById("myAudio");
    audioTrack = document.getElementById("myAudioTrack");

    audio.addEventListener('play', onPlay);
    audio.addEventListener('pause', onPause);
    audio.addEventListener('ended', onEnded);
    audio.addEventListener('timeupdate', onTimeupdate);
    audio.addEventListener('durationchange', onDurationupdate);
    audio.addEventListener('error', onError);
  }
}

function onPlay() {
  audio.play();
  onStateChange(playing);
}

function onPause() {
  audio.pause();
  change_time_state = true;

  onStateChange(pause);
}

function onEnded() {
  onCurrentTimeChange(0);
  onDurationChange(0);
  onStateChange(ended);
}

var previousTime;
function onTimeupdate() {
  var currentTime = parseInt(audio.currentTime);
  if (currentTime && previousTime != currentTime) {
    previousTime = currentTime;
    onCurrentTimeChange(currentTime);
  }
}

var previousDuration;
function onDurationupdate() {
  var currentDuration = parseInt(audio.duration);
  if (currentDuration && previousDuration != currentDuration) {
    previousDuration = currentDuration;
    onDurationChange(currentDuration);
  }
}

function onError() {
  console.log(audio.error);

  if (currAudioStreams.length > 0 && currAudioStreams.length > currAudioIndex) {
    currAudioIndex++;
    loadAudio(currAudioStreams[currAudioIndex]);
    seekTo(previousTime);
  }
}

var currAudioIndex = 0;
var currAudioStreams = [];

async function loadVideoById(id) {
  stop();
  var [videoStreams, audioStreams] = await getVideoInfo(id);

  //currVideoStreams = videoStreams;
  currAudioStreams = audioStreams;

  //loadVideo(currVideoStreams[0]);
  loadAudio(currAudioStreams[0]);
  playVideo();
}

function loadAudio(a) {
  audio.src = a.url;
  audio.type = a.mime;
}

function playVideo() {
  audio.play();
}

function pauseVideo() {
  audio.pause();
}

function stop() {
  audio.src = "";
  currAudioStreams = [];
  currAudioIndex = 0;
  previousDuration = 0;
  previousTime = 0;
  onCurrentTimeChange(0);
  onDurationChange(0);
}

function seekTo(position) {
  audio.currentTime = position;
}

function mute() {
  audio.muted = true;
}

function unMute() {
  audio.muted = false;
}
