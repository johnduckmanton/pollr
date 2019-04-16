  
Param(
    [parameter(Mandatory=$false)][string]$acrName,
    [parameter(Mandatory=$false)][string]$gitUser,
    [parameter(Mandatory=$false)][string]$repoName="pollr",
    [parameter(Mandatory=$false)][string]$gitBranch="master",
    [parameter(Mandatory=$true)][string]$patToken
)

$gitContext = "https://github.com/$gitUser/$repoName"

$services = @( 
    @{ Name="pollrapi"; Image="pollr/api"; File="/Pollr.API/Dockerfile" },
    @{ Name="pollradminui"; Image="pollr/adminui"; File="/Pollr.AdminUI/Dockerfile" },
    @{ Name="pollrui"; Image="pollr/ui"; File="/Pollr.UI/Dockerfile" }
)

$services |% {
    $bname = $_.Name
    $bimg = $_.Image
    $bfile = $_.File
    Write-Host "Setting ACR build $bname ($bimg)"    
    az acr build-task create --registry $acrName --name $bname --image ${bimg}:$gitBranch --context $gitContext --branch $gitBranch --git-access-token $patToken --file $bfile
}