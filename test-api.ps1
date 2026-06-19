$h = 'https://fakestoreapi.com'

Write-Host "=== GET /products/1 (original) ==="
$p = Invoke-RestMethod -Uri ($h + '/products/1') -Method Get
Write-Host ("title: " + $p.title)
Write-Host ("price: " + $p.price)

Write-Host "`n=== PUT /products/1 (modify) ==="
$body = @{
    title       = 'MODIFICADO_POR_TP4'
    price       = 999.99
    description = $p.description
    category    = $p.category
    image       = $p.image
} | ConvertTo-Json
$r = Invoke-RestMethod -Uri ($h + '/products/1') -Method Put -Body $body -ContentType 'application/json'
Write-Host ("PUT returned title: " + $r.title)
Write-Host ("PUT returned price: " + $r.price)

Write-Host "`n=== GET /products/1 (after PUT) ==="
$p2 = Invoke-RestMethod -Uri ($h + '/products/1') -Method Get
Write-Host ("title: " + $p2.title)
Write-Host ("price: " + $p2.price)

Write-Host "`n=== GET /products (count) ==="
$all = Invoke-RestMethod -Uri ($h + '/products') -Method Get
Write-Host ("count: " + $all.Count)
