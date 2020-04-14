var patDecrypationJsFile = /\/s\/player\/([a-zA-Z0-9]+)\/player_ias([a-zA-Z0-9_./]+)\.js/
var pathSignatureDecFunction = /\b([\w$]{2})\s*=\s*function\((\w+)\)\{\s*\2=\s*\2\.split\(\"\"\)\s*;/;

let nodeHttpReq = async function (url) {
    return new Promise((resolve) => {
        request.get({
            uri: url
        }, function (err, response, body) {
            if (err || parseInt(response.statusCode / 100) != 2)
                throw new Error(err);
            else
                resolve(body);
        });
    });
}

let jsHttpReq = async function (url) {
    return new Promise((resolve) => {
        var req = new XMLHttpRequest();
        req.onreadystatechange = function () {
            if (req.readyState === XMLHttpRequest.DONE) {
                if (req.status / 100 == 2)
                    resolve(req.responseText)
                else
                    throw new Error(req.responseText);
            }
        };
        req.open('GET', url, true);
        req.send(null);
    });
}

let httpReq;
let request;

if (typeof window === 'undefined') {
    request = require("request");
    httpReq = nodeHttpReq;
}
else {
    httpReq = jsHttpReq;
}

function getValueFromQuery(url, key) {
    var res = url.split('&');
    for (var prop in res) {
        var query = res[prop].split('=');
        if (query[0] === key)
            return query[1];
    }
}

function getVideoInfoUrl(id) {
    return "https://www.youtube.com/get_video_info?video_id=" + id + "&eurl=" +
        encodeURI("https://www.youtube.googleapis.com/v/" + id);
}

async function getVideoInfo(id) {
    var response = await httpReq(getVideoInfoUrl(id));

    var issuccess = getValueFromQuery(response, "status");
    if (issuccess != "ok")
        return {};

    var value = getValueFromQuery(response, "player_response");
    var jobj = decodeURIComponent(value).replace('\\u0026', '&');
    var videoInfo = JSON.parse(jobj);
    return await parseVideoStreams(id, videoInfo);
}

async function parseVideoStreams(id, obj) {
    var formats = obj.streamingData.adaptiveFormats;
    var urls = {};
    var ciphers = {};

    for (var prop in formats) {
        var fmt = formats[prop];
        var itag = fmt.itag;
        var url = "";

        if ("cipher" in fmt) {
            ciphers[itag] = {
                cipher: decodeURIComponent(getValueFromQuery(fmt.cipher, "s")),
                curl: decodeURIComponent(getValueFromQuery(fmt.cipher, "url"))
            };
        }

        if ("url" in fmt) {
            url = fmt.url;
        }

        urls[itag] = {
            "url": url,
            "mime": fmt.mimeType,
            "quality": fmt.quality,
            "qualityLabel": fmt.qualityLabel,
            "fps": fmt.fps,
            "bitrate": fmt.bitrate
        };
    }

    var deciphers = await decipher(id, ciphers);
    for (var prop in deciphers) {
        urls[prop]["url"] = deciphers[prop];
    }

    return sortStreams(urls);
}

function getQualityRate(stream) {
    var rate = 0;

    var str = stream.quality;
    if (str == "hd1080")
        rate += 12;
    else if (str == "hd720")
        rate += 10;
    else if (str == "large")
        rate += 8;
    else if (str == "medium")
        rate += 6;
    else if (str == "small")
        rate += 4;
    else if (str == "tiny")
        rate += 2;

    return rate;
}

function sortStreams(streams) {
    var audioStreams = [];
    var videoStreams = [];

    for (var prop in streams) {
        var stream = streams[prop];

        if (stream.mime.indexOf("video") != -1) {
            videoStreams.push(stream);
        }
        else {
            audioStreams.push(stream);
        }
    }

    for (var i = 0; i < videoStreams.length; i++) {
        videoStreams.sort(function (a, b) {
            var aQ = getQualityRate(a);
            var bQ = getQualityRate(b);

            if (aQ == bQ) {
                if (a.fps == b.fps)
                    return b.bitrate - a.bitrate;
                else
                    return b.fps - a.fps;
            }
            else
                return bQ - aQ;
        });
    }

    for (var i = 0; i < audioStreams; i++) {
        audioStreams.sort(function (a, b) {
            return b.bitrate - a.bitrate;
        });
    }

    return [videoStreams, audioStreams];
}

var currDecipherJsUrl = " ";
var currDecipherScript = "";
var currDecipherFuncName = "";

async function decipher(id, encs) {
    var url = "https://youtube.com/watch?v=" + id;

    var response = await httpReq(url);
    var matchs = patDecrypationJsFile.exec(response);

    console.log(matchs);
    var jsPath = matchs[0];
    var jsUrl = "https://youtube.com" + jsPath;
    console.log(jsUrl);

    if (currDecipherJsUrl != jsUrl) {
        currDecipherJsUrl = jsUrl;

        var jresponse = await httpReq(jsUrl);
        var jsSource = jresponse;

        var braceStartRegex = /{/g;
        var match = braceStartRegex.exec(jsSource);
        match = braceStartRegex.exec(jsSource);

        var braceStartIndex = match.index;
        var braceEndIndex = jsSource.lastIndexOf('}');

        currDecipherScript = jsSource.substring(braceStartIndex + 1, braceEndIndex - 1);

        var funcMatch = pathSignatureDecFunction.exec(jsSource);
        currDecipherFuncName = funcMatch[1];
    }

    function getUrl(sig, url) {
        return url += ("&sig=" + encodeURIComponent(sig));
    }

    var dScript =
        "'use strict';var g={};" +
        currDecipherScript +
        "var dResult={};" +
        "for(var i in encs){" +
        "dResult[i] = getUrl(" + currDecipherFuncName + "(encs[i]['cipher']), encs[i]['curl']);" +
        "}dResult";

    return eval(dScript);
}