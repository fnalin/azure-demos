#Executar functions em outra portas:
#func host start --pause-on-error --port 5860

$produto1 = New-Guid
$produto2 = New-Guid
$produto3 = New-Guid
$clienteId = New-Guid
$body = '{
    "Produtos":[
            "' + $produto1 + '",
            "' + $produto2 + '",
            "' + $produto3 + '"],
    "ClienteId":"' + $clienteId +'",
    "Email":"nalin.fansoft.com.br",
    "Valor":100.15
}'

Invoke-WebRequest -Method Post `
    -Uri        'http://localhost:5860/api/OnReceberPedido' `
    -Headers    @{"Content-Type"="application/json"} `
    -Body       $body