
from flask import Flask, jsonify, request
import cv2
from ultralytics import YOLO
from paddleocr import PaddleOCR



ocr = PaddleOCR(use_angle_cls=True)
model = YOLO("best.pt")

def LicensePlate(urlImage):

    urlImage = urlImage.replace('\\', '/')
    Ivehicle = cv2.imread(urlImage)
    results = model.predict(source=Ivehicle, conf=0.5)

    result = results[0]
    if len(result.boxes) == 0:
        return "No License"
    else:
        box = result.boxes[0]

        xmin = int(box.xyxy[0][0])
        ymin = int(box.xyxy[0][1])
        xmax = int(box.xyxy[0][2])
        ymax = int(box.xyxy[0][3])
        cropped_image = Ivehicle[ymin:ymax, xmin:xmax].copy()

        # Perform OCR on the image bytes
        result = ocr.ocr(cropped_image)
        output = ""
        for row in result[0]:
            output+=row[1][0]

        return output


# Create an instance of Flask
app = Flask(__name__)

@app.route('/get_license_number', methods=['GET'])
def get_license_number():
    # Retrieve the string from the query parameter
    received_string = request.args.get('url_image')
    result = LicensePlate(received_string)
    data = {'LicenseNumber': result}
    print("Result:", result)
    return jsonify(data)
# Run the Flask app
if __name__ == '__main__':
    app.run(debug=True)
