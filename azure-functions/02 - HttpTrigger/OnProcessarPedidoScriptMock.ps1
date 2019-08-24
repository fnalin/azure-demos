Invoke-WebRequest -Method Post `
    -Uri 'http://localhost:7071/api/OnProcessarPedido' `
    -Headers @{"Content-Type"="application/json"} `
    -Body   '{
                "Produtos":[
                        "222dc68d-45e5-4004-83cc-e69496be63d2",
                        "346bbdb9-85c8-4d2e-9383-e1fce9928a89",
                        "b3013b87-fe00-4ebe-8304-019b17f727f9"],
                "Email":"nalin.fansoft.com.br",
                "ValorTotal":100.15
            }'
