# Windows 10 Setup

## Prerequisites

* Windows 10 with WSL2 installed
* Visual Studio 2022 (optional)
* Docker Desktop for Windows

## 1. Ensure that the simulator is running.

* Navigate to <https://10.202.26.169/simulator/> and make sure it's there.

![](file:///G:/PullmanGit/NUCLIO/NuclioDotNetTutorials/Part-4/img/sim-screenshot.png)

* If it isn't installed, get it from <https://artifactory.metro.ad.selinc.com:443/artifactory/edapt-docker-dev/simulator/0.0.22/>

## 2. Get and start the Nuclio Dashboard

`docker run -p 8070:8070 -v /var/run/docker.sock:/var/run/docker.sock -v /tmp:/tmp --name nuclio-dashboard quay.io/nuclio/dashboard:stable-amd64`

## 3a. Pull the docker container for the NuclioFunctions Functions.execute function.

`docker pull lakeside-docker-dev.artifactory.metro.ad.selinc.com/scaleby100:0.0.1`

### Get the `nuctl` Nuclio CLI

### Deploy the container to the Nuclio server (from WSL)

``
## 3b. Load the NuclioFunctions Visual Studio solution and build it.

`git clone `

## 4. Set up a Nats trigger on the Nuclio Dashboard


