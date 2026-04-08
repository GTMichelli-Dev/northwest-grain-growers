// Downloads a composite image: camera photo on the left, ticket info on the right
// Named: Ticket_XXXX_in.jpg or Ticket_XXXX_out.jpg
function downloadTicketImage(ticket, direction) {
    var img = document.getElementById('imageModalImg');
    var infoDiv = document.getElementById('imageModalInfo');
    if (!img || !img.naturalWidth) return;

    var imgW = img.naturalWidth;
    var imgH = img.naturalHeight;

    // Ticket panel width
    var panelW = 380;
    var padding = 24;
    var canvasW = imgW + panelW;
    var canvasH = Math.max(imgH, 500);

    var canvas = document.createElement('canvas');
    canvas.width = canvasW;
    canvas.height = canvasH;
    var ctx = canvas.getContext('2d');

    // Background
    ctx.fillStyle = '#f8f9fa';
    ctx.fillRect(0, 0, canvasW, canvasH);

    // Draw image on left, centered vertically
    var imgY = Math.max(0, (canvasH - imgH) / 2);
    ctx.drawImage(img, 0, imgY, imgW, imgH);

    // Draw ticket info panel on the right
    var panelX = imgW;

    // White background for panel
    ctx.fillStyle = '#ffffff';
    ctx.fillRect(panelX, 0, panelW, canvasH);

    // Border
    ctx.strokeStyle = '#333333';
    ctx.lineWidth = 2;
    ctx.strokeRect(panelX + 10, 10, panelW - 20, canvasH - 20);

    // Render ticket info by reading the DOM children properly
    var x = panelX + padding;
    var y = padding + 16;
    var maxW = panelW - (padding * 2);
    var lineH = 22;
    var labelCol = 140; // fixed width for labels

    var children = infoDiv.children;
    for (var i = 0; i < children.length; i++) {
        var el = children[i];
        var style = el.getAttribute('style') || '';

        // Header (centered, uppercase text)
        if (style.indexOf('text-align:center') >= 0) {
            ctx.font = 'bold 14px Courier New';
            ctx.fillStyle = '#333';
            ctx.textAlign = 'center';
            ctx.fillText(el.textContent.trim(), panelX + panelW / 2, y);
            y += 6;
            // Underline
            ctx.beginPath();
            ctx.moveTo(x, y);
            ctx.lineTo(x + maxW, y);
            ctx.lineWidth = 2;
            ctx.strokeStyle = '#333';
            ctx.stroke();
            y += lineH;
            continue;
        }

        // Weight section (has dashed borders)
        if (style.indexOf('border-top:1px dashed') >= 0) {
            // Draw top dashed line
            y -= 4;
            drawDashedLine(ctx, x, y, x + maxW, y);
            y += 14;

            // Render weight rows inside this div
            var weightDivs = el.querySelectorAll('div');
            for (var j = 0; j < weightDivs.length; j++) {
                var wd = weightDivs[j];
                var wStyle = wd.getAttribute('style') || '';
                var spans = wd.querySelectorAll('span');
                if (spans.length < 2) continue;

                var lbl = spans[0].textContent.trim();
                var val = spans[1].textContent.trim();

                if (wStyle.indexOf('font-size:16px') >= 0) {
                    // Bold net weight
                    ctx.font = 'bold 16px Courier New';
                    ctx.fillStyle = '#000';
                } else {
                    ctx.font = '13px Courier New';
                    ctx.fillStyle = '#333';
                }

                ctx.textAlign = 'left';
                ctx.fillText(lbl, x, y);
                ctx.textAlign = 'right';
                ctx.fillText(val, x + maxW, y);
                y += lineH;
            }

            // Draw bottom dashed line
            y += 2;
            drawDashedLine(ctx, x, y, x + maxW, y);
            y += 12;
            continue;
        }

        // Regular row with label:value spans
        var spans = el.querySelectorAll('span');
        if (spans.length >= 2) {
            var label = spans[0].textContent.trim();
            var value = spans[1].textContent.trim();

            ctx.font = 'bold 13px Courier New';
            ctx.fillStyle = '#333';
            ctx.textAlign = 'left';
            ctx.fillText(label, x, y);

            ctx.font = '13px Courier New';
            ctx.textAlign = 'right';
            ctx.fillText(value, x + maxW, y);
            y += lineH;
            continue;
        }

        // Plain text
        var text = el.textContent.trim();
        if (text) {
            ctx.font = '13px Courier New';
            ctx.fillStyle = '#333';
            ctx.textAlign = 'left';
            ctx.fillText(text, x, y);
            y += lineH;
        }
    }

    // Convert to blob and download
    canvas.toBlob(function (blob) {
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'Ticket_' + ticket + '_' + direction + '.jpg';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(a.href);
    }, 'image/jpeg', 0.92);
}

function drawDashedLine(ctx, x1, y1, x2, y2) {
    ctx.beginPath();
    ctx.setLineDash([4, 4]);
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.lineWidth = 1;
    ctx.strokeStyle = '#000';
    ctx.stroke();
    ctx.setLineDash([]);
}
